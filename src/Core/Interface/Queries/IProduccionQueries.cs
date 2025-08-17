using BTE_group_net_worker.Models.VisualModels;

namespace BTE_group_net_worker.Core.Interface.Queries
{
    public interface IProduccionQueries
    {
        Task<List<VistaReales>> ProduccionByFechaAndMaquina(DateTime fecha, string connectionString, int maquina);
    }
}
