using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Models;
using SqlKata;

namespace BTE_group_net_worker.Queries
{
    public class ConsolidadoQueries : IConsolidadoQueries
    {
        private readonly IDapper _db;

        public ConsolidadoQueries(IDapper db)
        {
            _db = db;
        }


        public async Task<Consolidado> ConsolidadpByFechaAndMaquina(DateTime fecha, int maquina, string connectionString)
        {
            try
            {
                Query query = _db.AsQuery<Consolidado>();
                query.Where("Fecha", fecha);
                query.Where("Maquina", maquina);
                var result = await _db.QueryFirstOrDefaultAsync<Consolidado>(query, connectionString);
                return result;
            }
            catch 
            {
                throw;
            }
        }
    }
}
