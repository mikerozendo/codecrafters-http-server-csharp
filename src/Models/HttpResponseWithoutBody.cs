namespace codecrafters_http_server.src.Models;

public static class HttpResponseWithoutBody
{
    public const string Http200OkResponse = "HTTP/1.1 200 OK\r\n\r\n";
    public const string Http404NotFoudResponse = "HTTP/1.1 404 Not Found\r\n\r\n";
}
