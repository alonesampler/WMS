using Microsoft.EntityFrameworkCore;
using WMS.Domain.Repositories;
using WMS.Domain.UnitOfWork;
using WMS.Infrastructure;
using WMS.Infrastructure.Persistens;
using WMS.Infrastructure.Persistens.Repositories;

namespace WMSystem.DI;

internal static class InfrastructureConfigurator
{
    public static void ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connctionString = configuration.GetConnectionString("PostgresSql");

        services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseNpgsql(connctionString,
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", ApplicationDbContext.SCHEMA_NAME));
        });
        
        ConfigureRepositories(services, configuration);
    }
    
    private static void ConfigureRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IResourceRepository, ResourceRepository>();
        services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();
    }

}