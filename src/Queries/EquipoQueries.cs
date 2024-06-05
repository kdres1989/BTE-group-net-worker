using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Models;
using BTE_group_net_worker.Models.VisualModels;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                query.Select("Id AS Maquina", "Proceso", "SubProceso", "Planta", "Sucursal", "TipoProceso" );
                var result = await _db.QueryAsync<EquipoVM>(query, connectionString);
                return result.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<EquipoVM> GetMaquinasById(int maquina, string connectionString)
        {
            try
            {
                Query query = _db.AsQuery<Equipo>();
                query.Select("Id AS Maquina", "Proceso", "SubProceso", "Planta", "Sucursal", "TipoProceso");
                query.Where("Id", maquina);
                var result = await _db.QueryFirstAsync<EquipoVM>(query, connectionString);
                return result;
            }
            catch
            {
                throw;
            }
        }


    }
}
