using Microsoft.EntityFrameworkCore;
using WMS.Domain.Repositories;
using WMS.Infrastructure.Persistens;
using WMS.Infrastructure.Persistens.Repositories;

namespace WMSystem.DI;

internal static class InfrastructureConfigurator
{
    public static void ConfigureTenantsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connctionString = configuration.GetConnectionString("Postgres");

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
    }

}