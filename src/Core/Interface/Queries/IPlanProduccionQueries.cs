using BTE_group_net_worker.Models.VisualModels;

namespace BTE_group_net_worker.Core.Interface.Queries
{
    public interface IPlanProduccionQueries
    {
        Task<VistaProduccion> PlanProduccionByFechaAndMaquina(DateTime fecha, string connectionString, int maquina, double TiempoDisponible, double TiempoNeto, double TiempoCalendario);
    }
}
