using Dapper.Contrib.Extensions;

namespace BTE_group_net_worker.Models
{
    [Table("tiempo")]
    public class Tiempo
    {
        [Key]
        public long Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Codigo { get; set; }
        public int Operario { get; set; }
        public int Maquina { get; set; }
        public bool Estado { get; set; }
        public string Observacion { get; set; }

    }
}
