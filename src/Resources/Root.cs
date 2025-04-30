using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Models;
using codecrafters_http_server.src.Utils;

namespace codecrafters_http_server.src.Resources;

public sealed class Root(IRequest request)
    : ResourceBase(request, new ConfiguredResource(ResourcePath.Root, HttpMethod.Get)), IResponseProducer
{
    public string ProduceResponse() => HttpResponseWithoutBody.Http200OkResponse;
}
