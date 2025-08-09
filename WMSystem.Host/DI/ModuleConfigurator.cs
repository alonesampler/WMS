using WMSystem.DI;

internal static class ModuleConfigurator
{
    public static IServiceCollection ConfigureModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureApplicationServices(configuration);
        services.ConfigureInfrastructure(configuration);
            
        return services;
    }
}