namespace codecrafters_http_server.src.Models;

public static class ResourcePath
{
    public static string Root { get; } = "/";
    public static string EchoWithParam { get; } = "/echo/{param}";
}
