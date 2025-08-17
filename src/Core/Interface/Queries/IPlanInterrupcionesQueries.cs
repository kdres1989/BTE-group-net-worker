using BTE_group_net_worker.Models.VisualModels;

namespace BTE_group_net_worker.Core.Interface.Queries
{
    public interface IPlanInterrupcionesQueries
    {
        Task<List<VistaParadas>> PlanInterrupcionesByFechaAndMaquina(DateTime fecha, string connectionString, int maquina, double TiempoDisponible);
    }
}
