using BTE_group_net_worker.Models;

namespace BTE_group_net_worker.Core.Interface.Repositories
{
    public interface IConsolidadoRepositories
    {
        Task<int> Insert(Consolidado consolidado, string connectionString);
        Task<bool> Update(Consolidado consolidado, string connectionString);

    }
}
