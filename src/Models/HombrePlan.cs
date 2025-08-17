using Dapper.Contrib.Extensions;

namespace BTE_group_net_worker.Models
{
    [Table("hombreplan")]
    public class HombrePlan
    {
        public long Id { get; set; }
        public int Maquina { get; set; }
        public int Personas { get; set; }
        public DateTime Fecha { get; set; }
    }
}
