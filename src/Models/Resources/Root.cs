using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Utils;

namespace codecrafters_http_server.src.Models.Resources;

public sealed class Root(IRequest request) : ResourceBase(request,
    ResourcePath.Root,
    new RequestedResource(ResourcePath.Root, HttpMethod.Get)
    ), IResponseProducer
{
    public string ProduceResponse() => HttpResponseWithoutBody.Http200OkResponse;
    public override bool HasMatchingRoute() => Request.GetRequestLine().Resource.Equals(ResourcePath.Root);
}
