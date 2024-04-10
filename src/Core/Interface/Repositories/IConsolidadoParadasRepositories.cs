using BTE_group_net_worker.Models;
namespace BTE_group_net_worker.Core.Interface.Repositories
{
    public interface IConsolidadoParadasRepositories
    {
        Task<int> Insert(ConsolidadoParadas consolidadoParadas, string connectionString);
        Task<bool> Delete(ConsolidadoParadas consolidadoParadas, string connectionString);
        Task<bool> BulkInsert(List<ConsolidadoParadas> consolidadoParadas, string connectionString);
    }
}
