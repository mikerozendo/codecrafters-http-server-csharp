using codecrafters_http_server.src.Exceptions;
using codecrafters_http_server.src.Interfaces;

namespace codecrafters_http_server.src.Models.RequestComponents;

public sealed class Line : IRequestComponent
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

        HttpMethod = new HttpMethod(requestLineArgs[0]);
        Resource = requestLineArgs[1];
        HttpVersion = requestLineArgs[2];
        return this;
    }
}