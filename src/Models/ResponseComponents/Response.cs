namespace codecrafters_http_server.src.Models.ResponseComponents;

public sealed class Response(StatusLine statusLine, Header header, object? body)
{
    public StatusLine StatusLine { get; set; } = statusLine;
    public Header Header { get; set; } = header;
    public object? Body { get; set; } = body;

    public override string ToString()
    {
        if (Body is null)
            return $"{StatusLine}{Header}";

        return $"{StatusLine}{Header}{Body}";
    }
}
