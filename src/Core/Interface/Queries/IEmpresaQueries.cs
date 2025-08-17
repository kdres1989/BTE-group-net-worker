namespace BTE_group_net_worker.Core.Interface.Queries
{
    public interface IEmpresaQueries
    {
        Task<List<string>> GetConexionEmpresas();
        Task<string> GetConexionbyId(int id);
    }
}
