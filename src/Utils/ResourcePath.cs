namespace codecrafters_http_server.src.Utils;

public static class ResourcePath
{
    public static string Root { get; } = "/";
    public static string EchoWithParam { get; } = "/echo/{path-param}";
    public static string UserAgent { get; } = "/user-agent";
    public static string Files { get; } = "/files/{path-param}";
}
