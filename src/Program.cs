using Autofac;
using Autofac.Extensions.DependencyInjection;
using BTE_group_net.Infrastructure.AutoFacModules;
using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker;
using BTE_group_net_worker.Core.Dapper;
using BTE_group_net_worker.Service;
using Serilog;

public static class Program
{
    public static IConfiguration _configuration;
    public static int Main(string[] args)
    {
        _configuration = GetConfiguration();
        Log.Logger = CreateSerilogLogger();

        try
        {
            Log.Information("Configurando Worker ({ApplicationContext})...", "Consolidar Turnos");
            CreateHostBuilder(args).Build().Run();
            return 0;
        }
        catch(Exception ex)
        {
            Log.Fatal("Programa Terminado Inexperadamente ({ApplicationContext})...", "Consolidar Turnos");
            return 1;
        }
        finally
        {
            Log.Information("Programa Terminado Inexperadamente ({ApplicationContext})...", "Consolidar Turnos");
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureContainer<ContainerBuilder>(container =>
        {
            container.RegisterModule(new ApplicationModule());
            container.RegisterModule(new QueriesModule());
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHttpClient();
            services.AddAutoMapper(typeof(Program));
            services.AddScoped<IDapper, EDapper>(d => new EDapper(_configuration.GetSection("ConnectionString").Value));
            services.AddHostedService<Worker>();
            services.AddHostedService<SocketService>();
        });

    private static Serilog.ILogger CreateSerilogLogger()
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(_configuration)
            .CreateLogger();
    }

    private static IConfiguration GetConfiguration() 
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        return builder.Build();
    }

}

