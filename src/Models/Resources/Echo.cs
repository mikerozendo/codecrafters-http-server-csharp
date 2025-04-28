using System.Net;
using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Models.ResponseComponents;
using codecrafters_http_server.src.Utils;

namespace codecrafters_http_server.src.Models.Resources;

public class Echo(IRequest request) : ResourceBase(request,
    ResourcePath.EchoWithParam.Split('/', StringSplitOptions.RemoveEmptyEntries)[0],
    new RequestedResource(ResourcePath.EchoWithParam, HttpMethod.Get)
    ), IResponseProducer
{
    public string ProduceResponse()
    {
        var requestLine = Request.GetRequestLine();
        var requestedPath = requestLine.Resource;

        var requestedPathArgs = requestedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        var plainTextResponse = requestedPathArgs[1];

        return new Response(
                    new StatusLine((int)HttpStatusCode.OK, "OK"),
                    new Header("text/plain", plainTextResponse.Length.ToString()),
                    plainTextResponse
                ).ToString();
    }

    public override bool HasMatchingRoute()
    {
        if (!HasMatchingHttpMethod()) return false;

        var requestedPath = Line.Resource;
        var requestedPathWithoutArgs = requestedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var requestedPathWithoutParam = requestedPathWithoutArgs[0];

        return RouteIdentifier.Equals(requestedPathWithoutParam) &&
               requestedPathWithoutArgs.Length == 2;
    }
}
