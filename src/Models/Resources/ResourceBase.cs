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
    public virtual bool HasMatchingRoute()
    {
        if (!HasMatchingHttpMethod()) return false;

        var requestedPath = Line.Resource.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (requestedPath is null)
            return false;

        if (requestedPath.Length < 1)
            return false;

        if (string.IsNullOrEmpty(requestedPath[0]))
            return false;

        return RouteIdentifier.Equals(requestedPath[0]);
    }
}
