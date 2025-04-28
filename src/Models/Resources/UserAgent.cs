using System.Net;
using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Models.ResponseComponents;
using codecrafters_http_server.src.Utils;

namespace codecrafters_http_server.src.Models.Resources;

public sealed class UserAgent(IRequest request) : ResourceBase(request,
    ResourcePath.UserAgent.Split('/', StringSplitOptions.RemoveEmptyEntries)[0],
    new RequestedResource(ResourcePath.UserAgent, HttpMethod.Get)
    ), IResponseProducer
{
    public string ProduceResponse()
    {
        return new Response(
            new StatusLine((int)HttpStatusCode.OK, "OK"),
            new Header("text/plain", Header.UserAgent.Length.ToString()),
            Header.UserAgent
        ).ToString();
    }
}
