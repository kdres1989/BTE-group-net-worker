using BTE_group_net_worker.Models.VisualModels;

namespace BTE_group_net_worker.Core.Interface.Queries
{
    public interface ITiempoQueries
    {
        Task<List<VistaParadas>> TiemposByFechaAndTipoDemora(DateTime fecha, int tipoDemora, string connectionString, int maquina);
    }
}
