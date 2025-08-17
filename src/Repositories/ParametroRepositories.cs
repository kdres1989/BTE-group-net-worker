using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Repositories;
using BTE_group_net_worker.Models;

namespace BTE_group_net_worker.Repositories
{
    public class ParametroRepositories : IParametroRepositories
    {
        private readonly IDapper _db;

        public ParametroRepositories(IDapper db)
        {
            _db = db;
        }


        public async Task<bool> Update(Parametro parametro)
        {
            try
            {
                return await _db.UpdateAsync(parametro);
            }
            catch
            {
                throw;
            }
        }

    }
}
