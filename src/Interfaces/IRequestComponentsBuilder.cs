namespace codecrafters_http_server.src.Interfaces;

public interface IRequestComponentsBuilder
{
    IEnumerable<IRequestComponent> BuildRequestComponents(string rawRequestString);
}
