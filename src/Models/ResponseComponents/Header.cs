namespace codecrafters_http_server.src.Models.ResponseComponents;

public sealed class Header(string contentType, string contentLength)
{
    private DateTime Date { get; set; } = DateTime.UtcNow;
    private string ContentType { get; set; } = contentType;
    private string ContentLength { get; set; } = contentLength;

    public override string ToString()
    {
        return $"Date: {Date}\r\nContent-Type: {ContentType}\r\nContent-Length: {ContentLength}\r\n\r\n";
    }
}
