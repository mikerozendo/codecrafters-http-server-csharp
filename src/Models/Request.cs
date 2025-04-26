namespace codecrafters_http_server.src.Models;

public sealed class Request : IRequest
{
    private readonly string _incommingRequestString;
    private readonly string[] _requestRawArgs;
    // public IEnumerable<IRequestComponent> RequestComponents { get; private set; } = [];

    public IEnumerable<IRequestComponent> Components { get; private set; } = [];

    public Request(IEnumerable<IRequestComponent> requestComponents)
    {
        Components = requestComponents ?? throw new ArgumentNullException(nameof(requestComponents));
    }
    public Request()
    {

    }
    // private readonly IReadOnlyDictionary<>
    // public RequestHeaders RequestHeaders { get; private set; }
    // public RequestLine RequestLine { get; private set; }

    // public Request(string incommingRequestString)
    // {
    //     if (string.IsNullOrWhiteSpace(incommingRequestString))
    //         throw new HttpRequestParsingException();

    //     _incommingRequestString = incommingRequestString;

    //     var args = _incommingRequestString.Split("\r\n");
    //     if (args.Length == 0)
    //         throw new HttpRequestParsingException();

    //     _requestRawArgs = args;
    // }


    // public void ParseRequest()
    // {
    //     var requestArgs = _incommingRequestString.Split("\r\n");

    //     if (requestArgs.Length < 1)
    //         throw new HttpRequestParsingException();

    //     RequestLine = new RequestLine(requestArgs[0]);
    //     RequestHeaders = new RequestHeaders(_incommingRequestString);
    // }

    // public void BuildRequestLine(string requestStringRaw)
    // {
    //     var lines = requestStringRaw.Split("\r\n");

    //     var requestLineLine = lines[0];
    //     if (lines.Length == 0)
    //         throw new HttpRequestParsingException();

    //     var requestLineArgs = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
    //     if (requestLineArgs.Length != 3)
    //         throw new HttpRequestParsingException();

    //     if (!Enum.TryParse<HttpMethod>(requestLineArgs[0], true, out var httpMethod))
    //         throw new HttpRequestParsingException();

    //     RequestLine = new RequestLine(httpMethod, requestLineArgs[1], requestLineArgs[2]);
    // }

    // public void BuildRequestHeaders()
    // {
    //     var lines = requestStringRaw.Split("\r\n");
    //     if (lines.Length == 0)
    //         throw new HttpRequestParsingException();

    //     var requestLineArgs = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
    //     if (requestLineArgs.Length != 3)
    //         throw new HttpRequestParsingException();

    //     if (!Enum.TryParse<HttpMethod>(requestLineArgs[0], true, out var httpMethod))
    //         throw new HttpRequestParsingException();

    //     RequestLine = new RequestLine(httpMethod, requestLineArgs[1], requestLineArgs[2]);
    // }
}
