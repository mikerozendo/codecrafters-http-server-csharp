using codecrafters_http_server.src.Interfaces;

namespace codecrafters_http_server.src.Models.RequestComponents;

public sealed class Request
{
    private IEnumerable<IRequestComponent> Components { get; set; } = [];

    public Request(IEnumerable<IRequestComponent> requestComponents)
    {
        Components = requestComponents ?? throw new ArgumentNullException(nameof(requestComponents));
    }

    public Line GetRequestLine()
    {
        return Components.OfType<Line>().Single();
    }

    public Header GetRequestHeaders()
    {
        return Components.OfType<Header>().Single();
    }

    public Request() { }
}
