using Autofac;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Queries;
using System.Diagnostics.CodeAnalysis;

namespace BTE_group_net.Infrastructure.AutoFacModules
{
    [ExcludeFromCodeCoverage]
    public class QueriesModule : Autofac.Module
    {
        public QueriesModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ParametroQueries>()
                .As<IParametroQueries>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EmpresaQueries>()
                .As<IEmpresaQueries>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EquipoQueries>()
                .As<IEquipoQueries>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TiempoQueries>()
                .As<ITiempoQueries>()
                .InstancePerLifetimeScope();
            builder.RegisterType<PlanParadasQueries>()
                .As<IPlanParadasQueries>()
                .InstancePerLifetimeScope();
            builder.RegisterType<PlanInterrupcionesQueries>()
                .As<IPlanInterrupcionesQueries>()
                .InstancePerLifetimeScope();
            builder.RegisterType<PlanProduccionQueries>()
                .As<IPlanProduccionQueries>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ProduccionQueries>()
                .As<IProduccionQueries>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ConsolidadoQueries>()
                .As<IConsolidadoQueries>()
                .InstancePerLifetimeScope();
            builder.RegisterType<HombrePlanQueries>()
                .As<IHombrePlanQueries>()
                .InstancePerLifetimeScope();
        }
    }
}
