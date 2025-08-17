using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Repositories;
using BTE_group_net_worker.Models;

namespace BTE_group_net_worker.Repositories
{
    public class ConsolidadoParadasRepositories : IConsolidadoParadasRepositories
    {
        private readonly IDapper _db;

        public ConsolidadoParadasRepositories(IDapper db)
        {
            _db = db;
        }

        public async Task<int> Insert(ConsolidadoParadas consolidadoParadas, string connectionString)
        {
            try
            {
                return await _db.InsertAsync(consolidadoParadas, connectionString);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(ConsolidadoParadas consolidadoParadas, string connectionString)
        {
            try
            {
                return await _db.DeleteAsync(consolidadoParadas, connectionString);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> BulkInsert(List<ConsolidadoParadas> consolidadoParadas, string connectionString)
        {
            try
            {
                var respuesta = await _db.BulkInsertAsync(consolidadoParadas, connectionString);
                return respuesta.Success;
            }
            catch
            {
                throw;
            }
        }

    }
}
