namespace codecrafters_http_server.src.Interfaces;

public interface IRequestParser<TComponent> where TComponent : IRequestComponent
{
    TComponent BuildFromRawString(string rawRequestString);
}
