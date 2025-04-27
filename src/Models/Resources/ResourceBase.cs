using codecrafters_http_server.src.Interfaces;

namespace codecrafters_http_server.src.Models.Resources;

public abstract class ResourceBase(IRequest request, string routeIdentifier, RequestedResource requestedResource)
{
    protected IRequest Request { get; set; } = request;
    public string RouteIdentifier { get; protected set; } = routeIdentifier;
    public RequestedResource RequestedResource { get; protected set; } = requestedResource;
    public abstract bool HasMatchingRoute();
}
