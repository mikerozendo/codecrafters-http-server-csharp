using codecrafters_http_server.src.Models.RequestComponents;

namespace codecrafters_http_server.src.Interfaces;

public interface IRequest
{
    IReadOnlyCollection<IRequestComponent> Components { get; }
    Line GetRequestLine();
    Header GetRequestHeaders();
    void BuildRequestComponents(string rawRequestString);
}
