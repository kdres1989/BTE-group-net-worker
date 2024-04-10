using Dapper.Contrib.Extensions;
using System.Reflection;

namespace BTE_group_net.Core
{
    public class Util
    {
        public static string GetTableName(Type type)
        {
            string empty = string.Empty;
            dynamic val = type.GetCustomAttribute<TableAttribute>(inherit: false)?.Name ?? ((dynamic)type.GetCustomAttributes(inherit: false).FirstOrDefault((object attr) => attr.GetType().Name == "TableAttribute"))?.Name;
            if (val != null)
            {
                empty = val;
            }
            else
            {
                empty = type.Name + "s";
                if (type.IsInterface && empty.StartsWith("I"))
                {
                    empty = empty.Substring(1);
                }
            }

            return empty;
        }
    }
}
