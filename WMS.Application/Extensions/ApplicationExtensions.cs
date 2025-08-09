using WMS.Application.DTOs.Resource.Response;
using WMS.Application.DTOs.UnitOfMeasure.Response;
using WMS.Domain.Entities;

namespace WMS.Application.Extensions;

internal static class ApplicationExtensions
{
    public static ResourceResponse ToResponse(this Resource resource) => 
        new ResourceResponse(resource.Id, resource.Title);
    
    public static UnitOfMeasureResponse ToResponse(this UnitOfMeasure unitOfMeasure) =>
        new UnitOfMeasureResponse(unitOfMeasure.Id, unitOfMeasure.Title);
}
