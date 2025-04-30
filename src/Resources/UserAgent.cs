using System.Net;
using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Models;
using codecrafters_http_server.src.Models.ResponseComponents;
using codecrafters_http_server.src.Utils;

namespace codecrafters_http_server.src.Resources;

public sealed class UserAgent(IRequest request) : ResourceBase(request, new ConfiguredResource(ResourcePath.UserAgent, HttpMethod.Get)), IResponseProducer
{
    public async Task<string> ProduceResponseAsync()
    {
        return await Task.FromResult(new Response(
            new StatusLine((int)HttpStatusCode.OK, "OK"),
            new Header("text/plain", Header.UserAgent.Length.ToString()),
            Header.UserAgent
        ).ToString());
    }
}
