namespace codecrafters_http_server.src.Models;

public class Resource(string path, HttpMethod httpMethod, object response, object? param = null)
{
    public string Path { get; set; } = path;
    public object? Param { get; set; } = param;
    public HttpMethod HttpMethod { get; set; } = httpMethod;
    public object Response { get; set; } = response;
}