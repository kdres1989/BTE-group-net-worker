using Dapper.Contrib.Extensions;

namespace BTE_group_net_worker.Models
{
    [Table("planproduccion")]
    public class PlanProduccion
    {
        [Key]
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public int StdPNeta { get; set; }
        public double Mix { get; set; }
    }
}
