using Dapper.Contrib.Extensions;

namespace BTE_group_net_worker.Models
{
    [Table("consolidado")]
    public class Consolidado
    {
        [Key]
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Maquina { get; set; }
        public int SubProceso { get; set; }
        public int Proceso { get; set; }
        public int Planta { get; set; }
        public int Sucursal { get; set; }
        public double ProduccionPlan { get; set; }
        public double ProduccionReal { get; set; }
        public double ProduccionEstandar { get; set; } 
        public double TCalendario { get; set; }
        public double TDispPlan { get; set; }
        public double TDispReal { get; set; }
        public double TNetoPlan { get; set; }
        public double TNetoReal { get; set; }
        public double PNetaPlan { get; set; }
        public double PNetaReal { get; set; }
        public double PNetaEstandar { get; set; }
        public double UDispPlan { get; set; }
        public double UDispReal { get; set; }
        public double UNetoPlan { get; set; }
        public double UNetoReal { get; set; }
        public double PNetaMixReal { get; set; }
        public double PNetaMixEstandar { get; set; }

    }
}
