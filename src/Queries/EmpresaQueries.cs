using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Models;
using SqlKata;

namespace BTE_group_net_worker.Queries
{
    public class EmpresaQueries : IEmpresaQueries
    {
        private readonly IDapper _db;

        public EmpresaQueries(IDapper db)
        {
            _db = db;
        }

        public async Task<List<string>> GetConexionEmpresas()
        {
            try
            {
                Query query = _db.AsQuery<Empresa>();
                query.Select("ConnectionString");
                query.Where("Estado", true);
                var result = await _db.QueryAsync<string>(query);
                return result.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetConexionbyId(int id)
        {
            try
            {
                Query query = _db.AsQuery<Empresa>();
                query.Select("ConnectionString");
                query.Where("Id", id);
                var result = await _db.QueryFirstAsync<string>(query);
                return result;
            }
            catch
            {
                throw;
            }
        }

    }
}
