using BTE_group_net_worker.Models.VisualModels;

namespace BTE_group_net_worker.Core.Interface.Queries
{
    public interface IHombrePlanQueries
    {
        Task<HombrePlanVM> GetHombresbyFechaMaquina(int maquina, DateTime fecha, string connectionString);
    }
}
