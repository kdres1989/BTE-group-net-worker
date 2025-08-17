using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Repositories;
using BTE_group_net_worker.Models;
using SqlKata.Compilers;
using System.Text;

namespace BTE_group_net_worker.Repositories
{
    public class ConsolidadoInterrupcionesRepositories : IConsolidadoInterrupcionesRepositories
    {
        private readonly IDapper _db;
        private readonly SqlServerCompiler _Compiler;

        public ConsolidadoInterrupcionesRepositories(IDapper db)
        {
            _Compiler = new SqlServerCompiler();
            _db = db;
        }

        public async Task<bool> BulkInsert(List<ConsolidadoInterrupciones> consolidadoInterrupciones, string connectionString)
        {
            try
            {
                var respuesta = await _db.BulkInsertAsync(consolidadoInterrupciones, connectionString);
                return respuesta.Success;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteMultiple(int Maquina, DateTime Fecha, string connectionString)
        {
            bool rs = false;
            try
            {
                StringBuilder sqlString = new();
                sqlString.AppendFormat("DELETE FROM consolidadointerrupciones  WHERE Fecha = '{0}' and Maquina = {1};", Fecha.ToString("yyyy-MM-dd HH:mm:ss"), Maquina);
                sqlString.AppendLine();
                sqlString.AppendFormat("DELETE FROM consolidadoparadas WHERE Fecha = '{0}' and Maquina = {1}", Fecha.ToString("yyyy-MM-dd HH:mm:ss"), Maquina);
                await _db.ExecuteScalarAsync(sqlString.ToString(), connectionString);
                rs = true;
            }
            catch
            {

            }
            return rs;
        }


    }
}
