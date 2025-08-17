using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Repositories;
using BTE_group_net_worker.Models;
using SqlKata.Compilers;

namespace BTE_group_net_worker.Repositories
{
    public class ConsolidadoRepositories : IConsolidadoRepositories
    {
        private readonly IDapper _db;
        private readonly SqlServerCompiler _Compiler;

        public ConsolidadoRepositories(IDapper db)
        {
            _Compiler = new SqlServerCompiler();
            _db = db;
        }

        public async Task<int> Insert(Consolidado consolidado, string connectionString)
        {
            try
            {
                return await _db.InsertAsync(consolidado, connectionString);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(Consolidado consolidado, string connectionString)
        {
            try
            {
                return await _db.UpdateAsync(consolidado, connectionString);
            }
            catch
            {
                throw;
            }
        }


    }
}
