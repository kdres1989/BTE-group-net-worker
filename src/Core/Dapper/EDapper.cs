using BTE_group_net.Infrastructure.Interfaces.Core;
using Dapper.Contrib.Extensions;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlKata;
using System.Reflection;
using Dapper;
using BTE_group_net.Core;

namespace BTE_group_net_worker.Core.Dapper
{
    public class EDapper : IDapper
    {
        private string _ConnectionString = "ConnectionString";

        public EDapper(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        public string ReadDataBaseConnection()
        {
            string db = string.Empty;
            using (var d = new MySqlConnection(_ConnectionString))
            {
                db = d.Database;
            }
            return db;
        }

        public Query AsQuery<T>()
        {
            var db = new Query(GetTableName(typeof(T)));
            return db;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(Query q, string? ConnectionString = null)
        {
            using (var connection = new MySqlConnection(ConnectionString ?? _ConnectionString))
            {
                var compiler = new MySqlCompiler();
                var result = compiler.Compile(q);
                var factory = new QueryFactory(connection, compiler);
                return await factory.GetAsync<T>(q);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string q, string? ConnectionString = null)
        {
            using (var connection = new MySqlConnection(ConnectionString ?? _ConnectionString))
            {
                return await connection.QueryAsync<T>(q);
            }
        }



        public async Task<T> QueryFirstOrDefaultAsync<T>(Query q, string? ConnectionString = null)
        {
            using (var connection = new MySqlConnection(ConnectionString ?? _ConnectionString))
            {
                var compiler = new MySqlCompiler();
                var result = compiler.Compile(q);
                var factory = new QueryFactory(connection, compiler);
                return await factory.FirstOrDefaultAsync<T>(q);
            }
        }


        public async Task<T> QueryFirstAsync<T>(Query q, string? ConnectionString = null)
        {
            using (var connection = new MySqlConnection(ConnectionString ?? _ConnectionString))
            {
                var compiler = new MySqlCompiler();
                var result = compiler.Compile(q);
                var factory = new QueryFactory(connection, compiler);
                return await factory.FirstAsync<T>(q);
            }

        }

        public async Task<int> InsertAsync<T>(T entityToInsert, string? ConnectionString = null) where T : class
        {
            using (var connection = new MySqlConnection(ConnectionString ?? _ConnectionString))
            {
                return await connection.InsertAsync(entityToInsert);
            }
        }

        public async Task<bool> UpdateAsync<T>(T entityToUpdate, string? ConnectionString = null) where T : class
        {
            using (var connection = new MySqlConnection(ConnectionString ?? _ConnectionString))
            {
                return await connection.UpdateAsync(entityToUpdate);
            }
        }

        public async Task<bool> DeleteAsync<T>(T entityToDelete, string? ConnectionString = null) where T : class
        {
            using (var connection = new MySqlConnection(ConnectionString ?? _ConnectionString))
            {
                return await connection.DeleteAsync(entityToDelete);
            }
        }

        public async Task<object> ExecuteScalarAsync(string sql, string? ConnectionString = null)
        {
            using (var connection = new MySqlConnection(ConnectionString ?? _ConnectionString))
            {
                return await connection.ExecuteAsync(sql);
            }
        }

        public async Task<BulkStatics> BulkInsertAsync<T>(IEnumerable<T> entity, string? ConnectionString = null)
        {
            BulkStatics bulkStatics = new BulkStatics();
            using MySqlConnection connection = new MySqlConnection(ConnectionString ?? _ConnectionString);
            connection.Open();
            try
            {
                bulkStatics = await connection.InsertBulkAsync(entity);
                connection.Close();
                return bulkStatics;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on BulkInsert. ex=> " + ex.Message);
                bulkStatics.Exception = ex;
                connection.Close();
                return bulkStatics;
            }
        }


        public void Dispose() { }

        #region Metodos Privados

        private static string GetTableName(Type type)
        {
            string name = string.Empty;
            var tablrAttrName =
                type.GetCustomAttribute<TableAttribute>(false)?.Name
                ?? (type.GetCustomAttributes(false).FirstOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic)?.Name;
            if (tablrAttrName != null)
            {
                name = tablrAttrName;
            }
            else
            {
                name = type.Name + "s";
                if (type.IsInterface && name.StartsWith("I"))
                    name = name.Substring(1);
            }
            return name;

        }

        #endregion
    }
}
