using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Models.RequestComponents;

namespace codecrafters_http_server.src.Models.Resources;

public abstract class ResourceBase(IRequest request, string routeIdentifier, RequestedResource requestedResource)
{
    protected IRequest Request { get; private set; } = request;
    public string RouteIdentifier { get; protected set; } = routeIdentifier;
    private RequestedResource RequestedResource { get; set; } = requestedResource;
    protected Line Line { get; private set; } = request.GetRequestLine();
    protected Header Header { get; private set; } = request.GetRequestHeaders();

    protected bool HasMatchingHttpMethod() => RequestedResource.HttpMethod == Request.GetRequestLine().HttpMethod;
    public abstract bool HasMatchingRoute();
}
