using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Models;
using SqlKata;

namespace BTE_group_net_worker.Queries
{
    public class ParametroQueries : IParametroQueries
    {
        private readonly IDapper _db;

        public ParametroQueries(IDapper db)
        {
            _db = db;
        }

        public async Task<Parametro> GetParametroByNombre(string Nombre)
        {
            try
            {
                Query query = _db.AsQuery<Parametro>();
                query.Where("Nombre", Nombre);
                var result = await _db.QueryFirstOrDefaultAsync<Parametro>(query);
                return result;
            }
            catch 
            {
                throw;
            }
        }
    }
}
