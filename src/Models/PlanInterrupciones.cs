using Dapper.Contrib.Extensions;

namespace BTE_group_net_worker.Models
{
    [Table("planinterrupciones")]
    public class PlanInterrupciones
    {
        [Key]
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public int StdUNeta { get; set; }
        public double Porcentaje { get; set; }
    }
}
