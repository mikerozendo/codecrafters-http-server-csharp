using System.Net;
using System.Text;
using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Utils;

namespace codecrafters_http_server.src.Models.Resources;

public class Echo : ResourceBase, IResponseProducer
{
    public Echo(IRequest request) : base(request,
        ResourcePath.EchoWithParam.Split('/', StringSplitOptions.RemoveEmptyEntries)[0],
        new RequestedResource(ResourcePath.EchoWithParam, HttpMethod.Get)
    )
    {
    }

    public HttpResponseMessage ProduceResponse()
    {
        var requestLine = Request.GetRequestLine();
        var requestedPath = requestLine.Resource;

        var requestedPathArgs = requestedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        var plainTextResponse = requestedPathArgs[1];

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(plainTextResponse, Encoding.UTF8, "text/plain")
            {
                Headers =
                {
                    { "Content-Length", plainTextResponse.Length.ToString() }
                }
            }
        };
    }

    public override bool HasMatchingRoute()
    {
        var requestLine = Request.GetRequestLine();

        var requestedPath = requestLine.Resource;
        var requestedPathWithoutArgs = requestedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var requestedPathWithoutParam = requestedPathWithoutArgs[0];

        return RouteIdentifier.Equals(requestedPathWithoutParam) &&
               requestedPathWithoutArgs.Length == 2;
    }
}
