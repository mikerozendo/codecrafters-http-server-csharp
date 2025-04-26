namespace codecrafters_http_server.src.Models;

public interface IRequestParser<TComponent> where TComponent : IRequestComponent
{
    TComponent BuildFromRawString(string rawRequestString);
}
