namespace codecrafters_http_server.src.Models;

public sealed class RequestedResource(string path, HttpMethod method, object? param = null)
{
    public string Path { get; set; } = path;
    public object? Param { get; set; } = param;//must be modified to be an array
    public HttpMethod HttpMethod { get; set; } = method;
}