using codecrafters_http_server.src.Models.RequestComponents;

namespace codecrafters_http_server.src.Interfaces;

public interface IIncomingRequestComponents
{
    // IEnumerable<IRequestComponent> Components { get; }
    Line GetRequestLine();
    Header GetRequestHeaders();
}
