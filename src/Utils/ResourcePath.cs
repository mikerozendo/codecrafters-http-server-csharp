namespace codecrafters_http_server.src.Utils;

public static class ResourcePath
{
    public static string Root { get; } = "/";
    public static string EchoWithParam { get; } = "/echo/{param}";
    public static string UserAgent { get; } = "user-agent";
}
