namespace codecrafters_http_server.src.Models;

public interface IRequestComponentsBuilder
{
    IEnumerable<IRequestComponent> BuildRequestComponents(string rawRequestString);
}
