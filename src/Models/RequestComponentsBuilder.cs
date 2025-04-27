using codecrafters_http_server.src.Interfaces;

namespace codecrafters_http_server.src.Models;

public sealed class RequestComponentsBuilder(IEnumerable<IRequestComponent> requestComponents)
 : IRequestComponentsBuilder
{
    private readonly IEnumerable<IRequestComponent> _requestComponents = requestComponents;

    public IEnumerable<IRequestComponent> BuildRequestComponents(string rawRequestString)
    {
        return _requestComponents.Select(requestComponent => requestComponent.BuildFromRawString(rawRequestString));
    }
}
