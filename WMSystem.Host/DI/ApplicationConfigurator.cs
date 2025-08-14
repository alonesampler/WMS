using WMS.Application.Services;
using WMS.Application.Services.Abstractions;

namespace WMSystem.DI;

internal static class ApplicationConfigurator
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IResourceService, ResourcesService>();
        services.AddScoped<IUnitOfMeasureService, UnitOfMeasuresService>();
        services.AddScoped<IReceiptsService, ReceiptsService>();

        return services;
    }
}