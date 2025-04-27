using codecrafters_http_server.src.Exceptions;
using codecrafters_http_server.src.Interfaces;

namespace codecrafters_http_server.src.Models.RequestComponents;

public sealed class Header : IRequestComponent
{
    public string Host { get; private set; }
    public string UserAgent { get; private set; }
    public string Accept { get; private set; }

    public IRequestComponent BuildFromRawString(string rawRequestString)
    {
        var headersArgs = rawRequestString.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();// Skip the first line (request line)

        for (int i = 0; i < headersArgs.Length; i++)
        {
            var arg = headersArgs[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (arg.Length != 2)
                throw new HttpRequestParsingException();

            switch (arg[0].Replace(":", ""))
            {
                case "Host":
                    Host = arg[1];
                    break;
                case "User-Agent":
                    UserAgent = arg[1];
                    break;
                case "Accept":
                    Accept = arg[1];
                    break;
                default:
                    throw new HttpRequestParsingException();
            }
        }

        return this;
    }
}
