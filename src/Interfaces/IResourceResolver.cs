using codecrafters_http_server.src.Models;
namespace codecrafters_http_server.src.Interfaces;

public interface IResourceResolver
{
    ICollection<RequestedResource> AvailableResources { get; set; }
    RequestedResource? ResolveResource(string requestedResource, HttpMethod httpMethod);
}
