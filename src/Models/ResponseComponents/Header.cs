namespace codecrafters_http_server.src.Models.ResponseComponents;

public sealed class Header
{
    private DateTime Date { get; set; } = DateTime.UtcNow;

    public override string ToString() => $"Date: {Date}\r\n";
}
