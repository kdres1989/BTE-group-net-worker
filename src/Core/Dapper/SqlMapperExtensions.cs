using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace BTE_group_net.Core
{
    public static class SqlMapperExtensions
    {
        public delegate string ColumnNameMapperDelegate(PropertyInfo propertyInfo);

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ComputedProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        private static readonly ConcurrentDictionary<int, string> TypeColumnName = new ConcurrentDictionary<int, string>();

        public static ColumnNameMapperDelegate ColumnNameMapper;

        public static async Task<BulkStatics> InsertBulkAsync<T>(this IDbConnection connection, IEnumerable<T> entityToInsert, int? commandTimeout = null, int? batchBase = null)
        {
            Stopwatch t = new Stopwatch();
            t.Start();
            (List<(string, DynamicParameters)>, BulkStatics) tuple = GenerateBulkSql(connection, entityToInsert, batchBase.HasValue ? batchBase.Value : 1000);
            BulkStatics s = tuple.Item2;
            bool wasClosed = connection.State == ConnectionState.Closed;
            if (wasClosed)
            {
                connection.Open();
            }

            foreach (var item in tuple.Item1)
            {
                await connection.ExecuteAsync(item.Item1, item.Item2, null, commandTimeout, CommandType.Text);
                s.BatchCount++;
            }

            if (wasClosed)
            {
                connection.Close();
            }

            t.Stop();
            s.ElapsedTime = t;
            //Console.WriteLine($"Bulk Elapsed Time: {t.ElapsedMilliseconds} ms");
            s.Success = true;
            return s;
        }

        private static List<PropertyInfo> ComputedPropertiesCache(Type type)
        {
            if (ComputedProperties.TryGetValue(type.TypeHandle, out var value))
            {
                return value.ToList();
            }

            List<PropertyInfo> list = (from p in TypePropertiesCache(type)
                                       where p.GetCustomAttributes(inherit: true).Any((object a) => a is ComputedAttribute)
                                       select p).ToList();
            ComputedProperties[type.TypeHandle] = list;
            return list;
        }

        private static List<PropertyInfo> KeyPropertiesCache(Type type)
        {
            if (KeyProperties.TryGetValue(type.TypeHandle, out var value))
            {
                return value.ToList();
            }

            List<PropertyInfo> list = TypePropertiesCache(type);
            List<PropertyInfo> list2 = list.Where((PropertyInfo p) => p.GetCustomAttributes(inherit: true).Any((object a) => a is KeyAttribute)).ToList();
            if (list2.Count == 0)
            {
                PropertyInfo propertyInfo = list.Find((PropertyInfo p) => string.Equals(p.Name, "id", StringComparison.CurrentCultureIgnoreCase));
                if (propertyInfo != null && !propertyInfo.GetCustomAttributes(inherit: true).Any((object a) => a is ExplicitKeyAttribute))
                {
                    list2.Add(propertyInfo);
                }
            }

            KeyProperties[type.TypeHandle] = list2;
            return list2;
        }

        private static List<PropertyInfo> TypePropertiesCache(Type type)
        {
            if (TypeProperties.TryGetValue(type.TypeHandle, out var value))
            {
                return value.ToList();
            }

            PropertyInfo[] array = type.GetProperties().Where(new Func<PropertyInfo, bool>(IsWriteable)).ToArray();
            TypeProperties[type.TypeHandle] = array;
            return array.ToList();
        }

        private static bool IsWriteable(PropertyInfo pi)
        {
            List<object> list = pi.GetCustomAttributes(typeof(WriteAttribute), inherit: false).AsList();
            if (list.Count != 1)
            {
                return true;
            }

            return ((WriteAttribute)list[0]).Write;
        }

        private static string GetColumnName(PropertyInfo propertyInfo)
        {
            if (TypeColumnName.TryGetValue(propertyInfo.GetHashCode(), out var value))
            {
                return value;
            }

            if (ColumnNameMapper != null)
            {
                value = ColumnNameMapper(propertyInfo);
            }
            else
            {
                dynamic val = ((dynamic)propertyInfo.GetCustomAttributes(inherit: false).FirstOrDefault((object attr) => attr.GetType().Name == "ColumnNameAttribute"))?.Name;
                value = ((!((val != null) ? true : false)) ? propertyInfo.Name : ((string)val));
            }

            TypeColumnName[propertyInfo.GetHashCode()] = value;
            return value;
        }

        private static (List<(string, DynamicParameters)>, BulkStatics) GenerateBulkSql<T>(IDbConnection _, IEnumerable<T> entityToInsert, int batchBase = 1000)
        {
            List<(string, DynamicParameters)> list = new List<(string, DynamicParameters)>();
            BulkStatics bulkStatics = new BulkStatics();
            Type type = entityToInsert.GetType();
            TypeInfo typeInfo = type.GetTypeInfo();
            if (typeInfo.ImplementedInterfaces.Any((Type ti) => ti.IsGenericType() && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) || typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                type = type.GetGenericArguments()[0];
            }

            string tableName = Util.GetTableName(type);
            StringBuilder stringBuilder = new StringBuilder(null);
            List<PropertyInfo> first = TypePropertiesCache(type);
            List<PropertyInfo> first2 = KeyPropertiesCache(type);
            List<PropertyInfo> second = ComputedPropertiesCache(type);
            List<PropertyInfo> list2 = first.Except(first2.Union(second)).ToList();
            for (int i = 0; i < list2.Count; i++)
            {
                PropertyInfo propertyInfo = list2[i];
                stringBuilder.AppendFormat("{0}", GetColumnName(propertyInfo));
                if (i < list2.Count - 1)
                {
                    stringBuilder.Append(", ");
                }
            }

            StringBuilder stringBuilder2 = new StringBuilder(null);
            DynamicParameters dynamicParameters = new DynamicParameters();
            int num = 0;
            int num3 = (bulkStatics.BatchSize = batchBase / list2.Count);
            //Console.WriteLine($"Batch Base:{batchBase}");
            int j = 0;
            for (int num4 = entityToInsert.Count(); j < num4; j++)
            {
                T val = entityToInsert.ElementAt(j);
                stringBuilder2.Append("(");
                for (int k = 0; k < list2.Count; k++)
                {
                    PropertyInfo propertyInfo2 = list2[k];
                    object value = propertyInfo2.GetValue(val);
                    string text = $"@{GetColumnName(propertyInfo2)}{j}{k}";
                    stringBuilder2.Append(text);
                    if (propertyInfo2.PropertyType.IsValueType)
                    {
                        if (propertyInfo2.PropertyType == typeof(DateTime))
                        {
                            DateTime dateTime = Convert.ToDateTime(value);
                            dynamicParameters.Add(text, dateTime, DbType.DateTime);
                        }
                        else if (propertyInfo2.PropertyType == typeof(bool))
                        {
                            bool flag = Convert.ToBoolean(value);
                            dynamicParameters.Add(text, flag ? 1 : 0, DbType.Int32);
                        }
                        else
                        {
                            dynamicParameters.Add(text, value);
                        }
                    }
                    else
                    {
                        dynamicParameters.Add(text, value);
                    }

                    if (k < list2.Count - 1)
                    {
                        stringBuilder2.Append(", ");
                    }
                }

                stringBuilder2.Append("),");
                num++;
                if (num % num3 == 0)
                {
                    stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
                    stringBuilder2.Append(";");
                    string item = $"insert into {tableName} ({stringBuilder}) values {stringBuilder2}";
                    list.Add((item, dynamicParameters));
                    stringBuilder2.Clear();
                    dynamicParameters = new DynamicParameters();
                }

                if (num == num4)
                {
                    bulkStatics.TotalItems = num4;
                    stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
                    stringBuilder2.Append(";");
                    string item2 = $"insert into {tableName} ({stringBuilder}) values {stringBuilder2}";
                    list.Add((item2, dynamicParameters));
                    stringBuilder2.Clear();
                    dynamicParameters = new DynamicParameters();
                }
            }

            return (list, bulkStatics);
        }

        public static BulkStatics InsertBulk<T>(this IDbConnection connection, IEnumerable<T> entityToInsert, int? commandTimeout = null, int? batchBase = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            (List<(string, DynamicParameters)>, BulkStatics) tuple = GenerateBulkSql(connection, entityToInsert, batchBase.HasValue ? batchBase.Value : 1000);
            BulkStatics item = tuple.Item2;
            bool flag = connection.State == ConnectionState.Closed;
            if (flag)
            {
                connection.Open();
            }

            foreach (var item2 in tuple.Item1)
            {
                connection.Execute(item2.Item1, item2.Item2, null, commandTimeout, CommandType.Text);
                item.BatchCount++;
            }

            if (flag)
            {
                connection.Close();
            }

            stopwatch.Stop();
            item.ElapsedTime = stopwatch;
            //Console.WriteLine($"Bulk Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");
            item.Success = true;
            return item;
        }
    }
}
