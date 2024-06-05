using Dapper.Contrib.Extensions;

namespace BTE_group_net_worker.Models
{
    [Table("equipo")]
    public class Equipo
    {
        [Key]
        public int Id { get; set; }
        public string IdEquipo { get; set; }
        public string Maquina { get; set; }
        public string Marca { get; set; }
        public int Proceso { get; set; }
        public int Categoria { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        public bool Estado { get; set; }
        public DateTime Antiguedad { get; set; }
        public int SubProceso { get; set; }
        public int Planta { get; set; }
        public int Sucursal { get; set; }
        public int TipoProceso { get; set; }
    }
}
