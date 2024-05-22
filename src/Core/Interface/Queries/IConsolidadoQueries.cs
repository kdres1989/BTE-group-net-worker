using BTE_group_net_worker.Models;

namespace BTE_group_net_worker.Core.Interface.Queries
{
    public interface IConsolidadoQueries
    {
        Task<Consolidado>ConsolidadpByFechaAndMaquina(DateTime fecha, int maquina, string connectionString);
    }
}
