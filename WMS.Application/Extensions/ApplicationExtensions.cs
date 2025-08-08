using WMS.Application.DTOs.Resource.Responce;
using WMS.Domain.Entities;

namespace WMS.Application.Extensions;

internal static class ApplicationExtensions
{
    public static ResourceResponse ToResponce(this Resource resource) => 
        new ResourceResponse(resource.Id, resource.Title);
}
