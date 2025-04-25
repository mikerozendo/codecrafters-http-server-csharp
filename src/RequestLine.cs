using codecrafters_http_server.src.Exceptions;

namespace codecrafters_http_server.src;

public sealed class RequestLine
{
    public HttpMethod HttpMethod { get; set; }
    public string Resource { get; set; } = string.Empty;
    public string HttpVersion { get; set; } = string.Empty;

    public RequestLine(string requestStringRaw)
    {
        var lines = requestStringRaw.Split("\r\n");
        if (lines.Length == 0)
            throw new HttpRequestParsingException();

        var requestLineArgs = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (requestLineArgs.Length != 3)
            throw new HttpRequestParsingException();

        if (!Enum.TryParse<HttpMethod>(requestLineArgs[0], true, out var httpMethod))
            throw new HttpRequestParsingException();

        HttpMethod = httpMethod;
        Resource = requestLineArgs[1];
        HttpVersion = requestLineArgs[2];
    }
}

public enum HttpMethod
{
    GET,
    POST,
    PUT,
    DELETE,
    PATCH,
    OPTIONS,
    HEAD
}
