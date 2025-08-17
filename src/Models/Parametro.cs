using Dapper.Contrib.Extensions;

namespace BTE_group_net_worker.Models
{
    [Table("parametro")]
    public class Parametro
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }    

    }
}
