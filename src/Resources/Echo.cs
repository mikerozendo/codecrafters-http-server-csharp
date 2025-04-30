using System.Net;
using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Models;
using codecrafters_http_server.src.Models.ResponseComponents;
using codecrafters_http_server.src.Utils;

namespace codecrafters_http_server.src.Resources;

public sealed class Echo(IRequest request) : ResourceBase(request, new ConfiguredResource(ResourcePath.EchoWithParam, HttpMethod.Get)), IResponseProducer
{
    public async Task<string> ProduceResponseAsync()
    {
        var requestLine = Request.GetRequestLine();
        var requestedPath = requestLine.Resource;

        var requestedPathArgs = requestedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        var plainTextResponse = requestedPathArgs[1];

        return await Task.FromResult(new Response(
                    new StatusLine((int)HttpStatusCode.OK, nameof(HttpStatusCode.OK)),
                    new Header("text/plain", plainTextResponse.Length.ToString()),
                    plainTextResponse
                ).ToString());
    }
}
