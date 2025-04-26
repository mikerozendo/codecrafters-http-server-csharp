namespace codecrafters_http_server.src.Models;

public interface IRequest
{
    IEnumerable<IRequestComponent> Components { get; }
}
