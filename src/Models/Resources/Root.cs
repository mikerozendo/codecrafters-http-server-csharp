using System.Net;
using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Utils;

namespace codecrafters_http_server.src.Models.Resources;

public sealed class Root : ResourceBase, IResponseProducer
{
    public Root(IRequest request)
    : base(request,
        ResourcePath.Root,
        new RequestedResource(ResourcePath.Root, HttpMethod.Get)
    )
    {
    }

    public HttpResponseMessage ProduceResponse() => new HttpResponseMessage(HttpStatusCode.OK);
    public override bool HasMatchingRoute() => Request.GetRequestLine().Resource.Equals(ResourcePath.Root);
}
