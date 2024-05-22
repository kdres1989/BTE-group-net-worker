using BTE_group_net_worker.Models.VisualModels;

namespace BTE_group_net_worker.Core.Interface.Queries
{
    public interface IEquipoQueries
    {
        Task<List<EquipoVM>> GetMaquinas(string connectionString);
        Task<EquipoVM> GetMaquinasById(int maquina, string connectionString);
    }
}
