using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Utils;

namespace codecrafters_http_server.src.Models;

public class ResourceResolver : IResourceResolver
{
    public ICollection<RequestedResource> AvailableResources { get; set; } = [
        new RequestedResource(ResourcePath.Root, new HttpMethod("GET"), HttpResponseWithoutBody.Http200OkResponse),
        new RequestedResource(ResourcePath.EchoWithParam, new HttpMethod("GET"), "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length:{RESPONSE_LENGHT}\r\n\r\n{RESPONSE}"),
        new RequestedResource(ResourcePath.UserAgent, new HttpMethod("GET"), "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length:{RESPONSE_LENGHT}\r\n\r\n{RESPONSE}"),
    ];

    public RequestedResource? ResolveResource(string requestedResource, HttpMethod httpMethod)
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

        var resource = AvailableResources.SingleOrDefault(internalResource =>
                {
                    if (internalResource.HttpMethod != httpMethod)
                        return false;

                    var internalResourceArgs = internalResource.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);

                    if (internalResourceArgs.Length != requestedResourceArgs.Length ||
                        !internalResourceArgs.Any(x => x.Contains('{')))
                        return false;

                    bool isValid = true;

                    //trash solution, its working until now, but I dont figure it out about how to fix yet :(
                    for (int i = 0; i < internalResourceArgs.Length; i++)
                    {
                        if (internalResourceArgs[i].Equals("{param}"))
                            internalResourceArgs[i] = $"param-{requestedResourceArgs[i]}/";

                        else if (!internalResourceArgs[i].Equals(requestedResourceArgs[i]))
                        {
                            isValid = false;
                            break;
                        }
                    }

                    var convertedResource = string.Join(string.Empty, internalResourceArgs);

                    if (isValid)
                    {
                        var responsebody = string.Join(string.Empty, convertedResource.Split("param-")[1].TakeWhile(c => c != '/'));
                        internalResource.Response = internalResource.Response.ToString().Replace("{RESPONSE}", responsebody).Replace("{RESPONSE_LENGHT}", responsebody.Length.ToString());
                    }

                    return isValid;
                });

        if (resource is null)
            return null;


        return resource;
    }
}
