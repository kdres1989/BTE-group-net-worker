using BTE_group_net_worker.Models;

namespace BTE_group_net_worker.Core.Interface.Repositories
{
    public interface IParametroRepositories
    {
        Task<bool> Update(Parametro parametro);
    }
}
