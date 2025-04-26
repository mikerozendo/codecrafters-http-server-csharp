namespace codecrafters_http_server.src.Models;

public static class ResourceResolver
{
    public static IReadOnlyCollection<Resource> AvailableResources { get; } = [
        new Resource(ResourcePath.Root, HttpMethod.GET, HttpResponseWithoutBody.Http200OkResponse),
        new Resource(ResourcePath.EchoWithParam, HttpMethod.GET, "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length: 3\r\n\r\nabc"),
    ];

    public static Resource? ResolveResource(string requestedResource, HttpMethod httpMethod)
    {
        if (string.IsNullOrWhiteSpace(requestedResource))
            return null;

        var requestedResourceArgs = requestedResource.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (requestedResourceArgs.Length < 2)// would be nice to just return Root resource here :) 
        {
            return AvailableResources.SingleOrDefault(
                x => x.Path.Equals(requestedResource, StringComparison.OrdinalIgnoreCase) &&
                     x.HttpMethod == httpMethod
            );
        }

        return AvailableResources.SingleOrDefault(internalResource =>
        {
            if (internalResource.HttpMethod != httpMethod)
                return false;

            var internalResourceArgs = internalResource.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (internalResourceArgs.Length != requestedResourceArgs.Length ||
                !internalResourceArgs.Any(x => x.Contains('{')))
                return false;

            bool isValid = true;

            for (int i = 0; i < internalResourceArgs.Length; i++)
            {
                if (internalResourceArgs[i].Equals("{param}"))
                    internalResourceArgs[i] = $"param-{requestedResourceArgs[i]}";

                else if (!internalResourceArgs[i].Equals(requestedResourceArgs[i]))
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        });
    }
}
