using codecrafters_http_server.src.Interfaces;

namespace codecrafters_http_server.src.Models.RequestComponents;

public sealed class Request : IRequest
{
    public IReadOnlyCollection<IRequestComponent> Components { get { return (IReadOnlyCollection<IRequestComponent>)_components; } }
    private IEnumerable<IRequestComponent> _components { get; set; }

    public Request(IEnumerable<IRequestComponent> requestComponents)
    {
        _components = requestComponents ?? throw new ArgumentNullException(nameof(requestComponents));
    }

    public void BuildRequestComponents(string rawRequestString)
    {
        _components.ToList().ForEach(requestComponent => requestComponent.BuildFromRawString(rawRequestString));
    }

    public Line GetRequestLine()
    {
        return Components.OfType<Line>().Single();
    }

    public Header GetRequestHeaders()
    {
        return Components.OfType<Header>().Single();
    }
}
