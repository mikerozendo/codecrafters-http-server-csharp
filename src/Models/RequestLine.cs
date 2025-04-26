using codecrafters_http_server.src.Exceptions;

namespace codecrafters_http_server.src.Models;

public sealed class RequestLine : IRequestComponent
{
    public HttpMethod HttpMethod { get; private set; }
    public string Resource { get; private set; }
    public string HttpVersion { get; private set; }

    public IRequestComponent BuildFromRawString(string rawRequestString)
    {
        var lines = rawRequestString.Split("\r\n");
        if (lines.Length == 0)
            throw new HttpRequestParsingException();

        var requestLineArgs = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (requestLineArgs.Length != 3)
            throw new HttpRequestParsingException();

        if (!Enum.TryParse<HttpMethod>(requestLineArgs[0], true, out var httpMethod))
            throw new HttpRequestParsingException();

        return new RequestLine(httpMethod, requestLineArgs[1], requestLineArgs[2]);
    }

    internal RequestLine(HttpMethod httpMethod, string resource, string httpVersion)
    {
        HttpMethod = httpMethod;
        Resource = resource;
        HttpVersion = httpVersion;
    }

    public RequestLine() { }
}