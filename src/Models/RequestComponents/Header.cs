using codecrafters_http_server.src.Interfaces;

namespace codecrafters_http_server.src.Models.RequestComponents;

public sealed class Header : IRequestComponent
{
    public string Host { get; set; }
    public string UserAgent { get; set; }
    public string Accept { get; set; }

    public IRequestComponent BuildFromRawString(string rawRequestString)
    {
        return new Header();
    }

    internal Header(string host, string userAgent, string accept)
    {
        Host = host;
        UserAgent = userAgent;
        Accept = accept;
    }

    public Header() { }
}
