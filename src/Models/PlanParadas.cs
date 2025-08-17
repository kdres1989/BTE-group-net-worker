using Dapper.Contrib.Extensions;

namespace BTE_group_net_worker.Models
{
    [Table("planparadas")]
    public class PlanParadas
    {
        [Key]
        public long Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Codigo { get; set; }
        public int Maquina { get; set; }
        public double Duracion { get; set; }
    }
}
