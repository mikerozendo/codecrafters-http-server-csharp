namespace codecrafters_http_server.src.Models;

public interface IResourceResolver
{
    Resource? ResolveResource(string requestedResource, HttpMethod httpMethod);
}
