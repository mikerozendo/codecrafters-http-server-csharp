using codecrafters_http_server.src.Interfaces;

namespace codecrafters_http_server.src.Models;

public sealed class RequestedResource(string path, HttpMethod method, object response, object? param = null)
{
    public string Path { get; set; } = path;
    public object? Param { get; set; } = param;
    public HttpMethod HttpMethod { get; set; } = method;
    public IIncomingRequestComponents IncomingRequestComponents { get; set; }
    public object Response { get; set; } = response;

    // public void SetResponse(string response)
    // {
    //     Response = response;
    // }
}