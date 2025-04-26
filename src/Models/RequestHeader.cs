namespace codecrafters_http_server.src.Models;

public sealed class RequestHeader : IRequestComponent
{
    public string Host { get; set; }

    public string UserAgent { get; set; }
    public string Accept { get; set; }

    // internal RequestHeaders(string incommingRequestString)
    // {
    //     var requestArgs = incommingRequestString.Split("\r\n");
    // }


    IRequestComponent IRequestParser<IRequestComponent>.BuildFromRawString(string rawRequestString)
    {
        return new RequestHeader();
    }

    public RequestHeader() { }
}
