using Autofac;
using BTE_group_net_worker.Bridge;
using BTE_group_net_worker.Core.Interface.Bridge;
using BTE_group_net_worker.Core.Interface.Repositories;
using BTE_group_net_worker.Models;
using BTE_group_net_worker.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace BTE_group_net.Infrastructure.AutoFacModules
{
    [ExcludeFromCodeCoverage]
    public class ApplicationModule : Autofac.Module
    {
        public ApplicationModule() { 
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Bridge>()
                .As<IBridge>()
                .SingleInstance();
            builder.RegisterType<ParametroRepositories>()
                .As<IParametroRepositories>()
                .InstancePerLifetimeScope();
            builder.RegisterType<SocketConfig>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ConsolidadoParadasRepositories>()
                .As<IConsolidadoParadasRepositories>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ConsolidadoInterrupcionesRepositories>()
                .As<IConsolidadoInterrupcionesRepositories>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ConsolidadoRepositories>()
                .As<IConsolidadoRepositories>()
                .InstancePerLifetimeScope();
        }
    }
}
