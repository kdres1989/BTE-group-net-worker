using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Models;
using BTE_group_net_worker.Models.VisualModels;
using SqlKata;

namespace BTE_group_net_worker.Queries
{
    public class EquipoQueries : IEquipoQueries
    {
        private readonly IDapper _db;

        public EquipoQueries(IDapper db)
        {
            _db = db;
        }

        public async Task<List<EquipoVM>> GetMaquinas(string connectionString)
        {
            try
            {
                Query query = _db.AsQuery<Equipo>();
                query.Select("Id AS Maquina", "Proceso", "SubProceso", "Planta", "Sucursal", "TipoProceso", "TipoSubProceso", "Estado");
                query.Where("Estado", true);
                var result = await _db.QueryAsync<EquipoVM>(query, connectionString);
                return result.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<EquipoVM>> GetMaquinasById(List<int> maquina, string connectionString)
        {
            try
            {
                Query query = _db.AsQuery<Equipo>();
                query.Select("Id AS Maquina", "Proceso", "SubProceso", "Planta", "Sucursal", "TipoProceso", "TipoSubProceso", "Estado");
                query.WhereIn("Id", maquina);
                query.Where("Estado", true);
                var result = await _db.QueryAsync<EquipoVM>(query, connectionString);
                return result.ToList();
            }
            catch
            {
                throw;
            }
        }


    }
}
