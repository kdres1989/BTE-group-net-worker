using BTE_group_net_worker.Models;

namespace BTE_group_net_worker.Core.Interface.Repositories
{
    public interface IConsolidadoInterrupcionesRepositories
    {
        Task<bool> BulkInsert(List<ConsolidadoInterrupciones> consolidadoInterrupciones, string connectionString);
        Task<bool> DeleteMultiple(int Maquina, DateTime Fecha, string connectionString);
    }
}
