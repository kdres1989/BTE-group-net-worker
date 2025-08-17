using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTE_group_net_worker.Models
{
    [Table("empresa")]
    public class Empresa
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public string Responsable { get; set; }
        public string NumeroContacto { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        public string ConnectionString { get; set; }
        public string Nit { get; set; }
        public string CargoResponsable { get; set; }
        public string Realm { get; set; }
        public string Prefijo { get; set; }
        public string PathSocket { get; set; }
        public int TipoSuscripcion { get; set; }
        public int TiempoSuscripcion { get; set; }
        public DateTime FechaInicioSuscripcion { get; set; }
        public DateTime FechaFinSuscripcion { get; set; }
    }
}
