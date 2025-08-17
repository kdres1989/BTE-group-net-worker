using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Models;
using BTE_group_net_worker.Models.VisualModels;
using SqlKata;

namespace BTE_group_net_worker.Queries
{
    public class HombrePlanQueries : IHombrePlanQueries
    {
        private readonly IDapper _db;
        public HombrePlanQueries(IDapper db) 
        {
            _db = db;
        }

        public async Task<HombrePlanVM> GetHombresbyFechaMaquina(int maquina, DateTime fecha, string connectionString)
        {
            try
            {
                Query query = _db.AsQuery<HombrePlan>();

                query.Select(
                    "hombreplan.Personas as PersonasPlan",
                    "hombrereal.Personas as PersonasReal"
                );

                query.LeftJoin("hombrereal", j =>
                    j.On("hombreplan.Maquina", "hombrereal.Maquina")
                     .On("hombreplan.Fecha", "hombrereal.Fecha")
                );

                query.Where("hombreplan.Fecha", fecha);
                query.Where("hombreplan.Maquina", maquina);

                var result = await _db.QueryFirstOrDefaultAsync<HombrePlanVM>(query, connectionString);
                return result;

            }
            catch
            {
                throw;
            }
        }
    }
}
