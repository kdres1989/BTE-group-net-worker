using BTE_group_net_worker.Core.Interface.Bridge;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Core.Interface.Repositories;
using BTE_group_net_worker.Models;
using BTE_group_net_worker.Models.VisualModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Security.Cryptography;

namespace BTE_group_net_worker.Bridge
{
    public class Bridge : IBridge
    {
        private readonly ILogger<Bridge> _logger;
        private readonly IParametroQueries _parametroQueries;
        private readonly IParametroRepositories _parametroRepositories;
        private readonly IEmpresaQueries _empresaQueries;
        private readonly IEquipoQueries _equipoQueries;
        private readonly ITiempoQueries _tiempoQueries;
        private readonly IPlanParadasQueries _planParadasQueries;
        private readonly IPlanInterrupcionesQueries _planInterrupcionesQueries;
        private readonly IPlanProduccionQueries _planProduccionQueries;
        private readonly IProduccionQueries _produccionQueries;
        private readonly IConsolidadoQueries _consolidadoQueries;
        private readonly IConsolidadoRepositories _consolidadoRepositories;
        private readonly IConsolidadoInterrupcionesRepositories _consolidadoInterrupcionesRepositories;
        private readonly IConsolidadoParadasRepositories _consolidadoParadasRepositories;

        public Bridge(ILogger<Bridge> logger, IParametroQueries parametroQueries, IParametroRepositories parametroRepositories, 
                      IEmpresaQueries empresaQueries, IEquipoQueries equipoQueries, ITiempoQueries tiempoQueries, IPlanParadasQueries planParadasQueries,
                      IPlanInterrupcionesQueries planInterrupcionesQueries, IPlanProduccionQueries planProduccionQueries, IProduccionQueries produccionQueries,
                      IConsolidadoQueries consolidadoQueries, IConsolidadoRepositories consolidadoRepositories, IConsolidadoInterrupcionesRepositories consolidadoInterrupcionesRepositories,
                      IConsolidadoParadasRepositories consolidadoParadasRepositories)
        {
            _logger = logger;
            _parametroQueries = parametroQueries;
            _parametroRepositories = parametroRepositories;
            _empresaQueries = empresaQueries;
            _equipoQueries = equipoQueries;
            _tiempoQueries = tiempoQueries;
            _planParadasQueries = planParadasQueries;
            _planInterrupcionesQueries = planInterrupcionesQueries;
            _planProduccionQueries = planProduccionQueries;
            _produccionQueries = produccionQueries;
            _consolidadoQueries = consolidadoQueries;
            _consolidadoRepositories = consolidadoRepositories;
            _consolidadoInterrupcionesRepositories = consolidadoInterrupcionesRepositories;
            _consolidadoParadasRepositories = consolidadoParadasRepositories;
        }

        public async Task Run()
        {
            if(DateTime.Now.Hour == 23)
            {
                string format = "dd/MM/yyyy HH:mm:ss";
                Parametro parametro = await _parametroQueries.GetParametroByNombre("FechaUltimaEjecucionWorker");
                DateTime fechaActual = DateTime.Today;
                DateTime fechaUltimaEjecucion = DateTime.TryParseExact(parametro.Valor, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDateTime) ? parsedDateTime : DateTime.Today.AddDays(-1);
                if (fechaActual > fechaUltimaEjecucion)
                {
                    List<string> conexionEmpresas = await _empresaQueries.GetConexionEmpresas();
                    foreach(string conexion in conexionEmpresas)
                    {
                        List<EquipoVM> maquinas = await _equipoQueries.GetMaquinas(conexion);
                        foreach(EquipoVM maquina in maquinas)
                        {
                            await Calculos(conexion, maquina, fechaActual);
                        }
                    }
                    parametro.Valor = fechaActual.ToString("dd/MM/yyyy HH:mm:ss");
                    await _parametroRepositories.Update(parametro);
                }

            }
        }
        public async Task RunManual(ManualRequest manualRequest)
        {
            string conexion = await _empresaQueries.GetConexionbyId(manualRequest.Empresa);
            foreach (int idMaquina in manualRequest.Maquinas)
            {
                EquipoVM maquina = await _equipoQueries.GetMaquinasById(idMaquina, conexion);
                for (DateTime fecha = manualRequest.FechaInicio; fecha <= manualRequest.FechaFin; fecha = fecha.AddDays(1))
                {
                    await Calculos(conexion, maquina, fecha);
                }
            }
        }



        private async Task<List<string>> Calculos(string conexion, EquipoVM maquina, DateTime fecha)
        {
            List<string> Errores = new();
            try
            {
                List<VistaParadas> paradasReales = await _tiempoQueries.TiemposByFechaAndTipoDemora(fecha, 2, conexion, maquina.Maquina);
                List<VistaParadas> paradasPlanes = await _planParadasQueries.PlanParadasByFechaAndTipoDemora(fecha, conexion, maquina.Maquina);
                List<ConsolidadoParadas> consolidadoParadas = (from paradasReal in paradasReales
                                                               join paradasPlan in paradasPlanes
                                                               on paradasReal.Codigo equals paradasPlan.Codigo
                                                               into temp
                                                               from paradasPlan in temp.DefaultIfEmpty()
                                                               select new ConsolidadoParadas
                                                               {
                                                                   Codigo = paradasReal != null ? paradasReal.Codigo : paradasPlan.Codigo,
                                                                   Maquina = maquina.Maquina,
                                                                   Fecha = fecha,
                                                                   TiempoReal = paradasReal != null ? paradasReal.Duracion : 0,
                                                                   TiempoPlan = paradasPlan != null ? paradasPlan.Duracion : 0,
                                                               }).Union(
                                                                    from paradasPlan in paradasPlanes
                                                                    join paradasReal in paradasReales
                                                                    on paradasPlan.Codigo equals paradasReal.Codigo
                                                                    into temp
                                                                    from paradasReal in temp.DefaultIfEmpty()
                                                                    select new ConsolidadoParadas
                                                                    {
                                                                        Codigo = paradasPlan != null ? paradasPlan.Codigo : paradasReal.Codigo,
                                                                        Maquina = maquina.Maquina,
                                                                        Fecha = fecha,
                                                                        TiempoReal = paradasReal != null ? paradasReal.Duracion : 0,
                                                                        TiempoPlan = paradasPlan != null ? paradasPlan.Duracion : 0,
                                                                    }).GroupBy(r => new { r.Codigo }).Select(grp => grp.First()).ToList();

                double tdp = consolidadoParadas.Sum(cp => cp.TiempoPlan);
                double tdr = consolidadoParadas.Sum(cp => cp.TiempoReal);
                double TiempoDisponiblePlan = 24 - (tdp > 0 ? tdp : 24);
                double TiempoDisponibleReal = 24 - (tdr > 0 ? tdr : 24);
                TiempoDisponiblePlan = TiempoDisponiblePlan > 0 ? TiempoDisponiblePlan : TiempoDisponibleReal > 0 ? TiempoDisponibleReal : TiempoDisponiblePlan;

                List<VistaParadas> inteReales = await _tiempoQueries.TiemposByFechaAndTipoDemora(fecha, 3, conexion, maquina.Maquina);
                List<VistaParadas> intePlanes = await _planInterrupcionesQueries.PlanInterrupcionesByFechaAndMaquina(fecha, conexion, maquina.Maquina, TiempoDisponiblePlan);
                List<VistaParadas> intePlanesVoces = intePlanes.Select(voz => new VistaParadas{ Codigo = voz.Codigo, Duracion = voz.Duracion * TiempoDisponibleReal }).ToList();
                List<ConsolidadoInterrupciones> consolidadoInterrupciones = (from inteReal in inteReales
                                                                             join intePlan in intePlanesVoces
                                                                             on inteReal.Codigo equals intePlan.Codigo
                                                                             into temp
                                                                             from intePlan in temp.DefaultIfEmpty()
                                                                             
                                                                             select new ConsolidadoInterrupciones
                                                                             {
                                                                                 Codigo = inteReal != null ? inteReal.Codigo : intePlan.Codigo,
                                                                                 Maquina = maquina.Maquina,
                                                                                 Fecha = fecha,
                                                                                 TiempoReal = inteReal != null ? inteReal.Duracion : 0,
                                                                                 TiempoPlan = intePlan != null ? intePlan.Duracion : 0,
                                                                             }).Union(
                                                                                    from intePlan in intePlanesVoces
                                                                                    join inteReal in inteReales
                                                                                    on intePlan.Codigo equals inteReal.Codigo
                                                                                    into temp
                                                                                    from inteReal in temp.DefaultIfEmpty()

                                                                                    select new ConsolidadoInterrupciones
                                                                                    {
                                                                                        Codigo = intePlan != null ? intePlan.Codigo : inteReal.Codigo,
                                                                                        Maquina = maquina.Maquina,
                                                                                        Fecha = fecha,
                                                                                        TiempoReal = inteReal != null ? inteReal.Duracion : 0,
                                                                                        TiempoPlan = intePlan != null ? intePlan.Duracion : 0,
                                                                                    }).GroupBy(r => new { r.Codigo }).Select(grp => grp.First()).ToList();
                double TiempoNetoPlan = TiempoDisponiblePlan - (intePlanes.Sum(ci => (ci.Duracion * TiempoDisponiblePlan)));
                double TiempoNetoReal = TiempoDisponibleReal - (consolidadoInterrupciones.Sum(ci => ci.TiempoReal));

                VistaProduccion vistaProduccionPlan = await _planProduccionQueries.PlanProduccionByFechaAndMaquina(fecha, conexion, maquina.Maquina, 24 - tdp, TiempoNetoPlan, 24);
                if (vistaProduccionPlan == null)
                {
                    vistaProduccionPlan = new()
                    {
                        PNetaStd = 0,
                        PNetaStdEstandar = 0,
                        Produccion = 0,
                        ProduccionEstandar = 0
                    };
                }

                List<VistaReales> vistaReales = await _produccionQueries.ProduccionByFechaAndMaquina(fecha, conexion, maquina.Maquina);
                double ProduccionReal = vistaReales.Sum(p => p.Peso);
                double PNetaMixReal = vistaReales.Sum(p => p.Peso / ProduccionReal / p.ToneladasHora) == 0 ? 0 : 1 / vistaReales.Sum(p => p.Peso / ProduccionReal / p.ToneladasHora);
                double PNetaMixRealEstandar = vistaReales.Sum(p => p.Peso / ProduccionReal / p.ToneladasHoraEstandar) == 0 ? 0 : 1 / vistaReales.Sum(p => p.Peso / ProduccionReal / p.ToneladasHoraEstandar);
                ProduccionReal /= 1000;
                double PNetaReal = TiempoNetoReal == 0 ? 0 : ProduccionReal / TiempoNetoReal;

                Consolidado consolidado = await _consolidadoQueries.ConsolidadpByFechaAndMaquina(fecha, maquina.Maquina, conexion);
                bool existe = consolidado != null;
                Consolidado nuevoconsolidado = new()
                {
                    Fecha = fecha,
                    Maquina = maquina.Maquina,
                    SubProceso = maquina.SubProceso,
                    Proceso = maquina.Proceso,
                    Planta = maquina.Planta,
                    Sucursal = maquina.Sucursal,
                    ProduccionPlan = vistaProduccionPlan.Produccion,
                    ProduccionReal = ProduccionReal,
                    ProduccionEstandar = vistaProduccionPlan.ProduccionEstandar,
                    TCalendario = 24,
                    TDispPlan = 24 - tdp,
                    TDispReal = TiempoDisponibleReal,
                    TNetoPlan = TiempoNetoPlan,
                    TNetoReal = TiempoNetoReal,
                    PNetaPlan = vistaProduccionPlan.PNetaStd,
                    PNetaReal = PNetaReal,
                    PNetaEstandar = vistaProduccionPlan.PNetaStdEstandar,
                    UDispPlan = TiempoDisponiblePlan/24,
                    UDispReal = TiempoDisponibleReal/24,
                    UNetoPlan = TiempoDisponiblePlan == 0 ? 0 : TiempoNetoPlan / TiempoDisponiblePlan,
                    UNetoReal = TiempoDisponibleReal == 0 ? 0 :TiempoNetoReal / TiempoDisponibleReal,
                    PNetaMixReal = PNetaMixReal,
                    PNetaMixEstandar = PNetaMixRealEstandar,
                    TipoProceso = maquina.TipoProceso
                };
                if (existe)
                {
                    nuevoconsolidado.Id = consolidado.Id;
                    await _consolidadoRepositories.Update(nuevoconsolidado, conexion);
                }
                else
                {
                    nuevoconsolidado.Id = 0;
                    await _consolidadoRepositories.Insert(nuevoconsolidado, conexion);
                }
                await _consolidadoInterrupcionesRepositories.DeleteMultiple(maquina.Maquina, fecha, conexion);
                await _consolidadoInterrupcionesRepositories.BulkInsert(consolidadoInterrupciones, conexion);
                await _consolidadoParadasRepositories.BulkInsert(consolidadoParadas, conexion);  
            }
            catch
            {
                Console.WriteLine("Error");
            }
                return Errores;
            
        }



    }
    
}
