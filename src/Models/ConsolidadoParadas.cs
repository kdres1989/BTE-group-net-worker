using Dapper.Contrib.Extensions;

namespace BTE_group_net_worker.Models
{
    [Table("consolidadoparadas")]
    public class ConsolidadoParadas
    {
        [Key]
        public long Id { get; set; }
        public int Maquina { get; set; }
        public DateTime Fecha { get; set; }
        public double TiempoReal { get; set; }
        public double TiempoPlan { get; set; }
        public int Codigo { get; set; }
    }
}
