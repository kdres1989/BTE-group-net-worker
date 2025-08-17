using Dapper.Contrib.Extensions;

namespace BTE_group_net_worker.Models
{
    [Table("produccion")]
    public class Produccion
    {
        [Key]
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Turno { get; set; }
        public string Pedido { get; set; }
        public int Peso { get; set; }
        public int Maquina { get; set; }
        public int Operario { get; set; }

    }
}
