using System.Net;
using System.Text;
using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Models;
using codecrafters_http_server.src.Models.ResponseComponents;
using codecrafters_http_server.src.Utils;

namespace codecrafters_http_server.src.Resources;

public sealed class Files(IRequest request, Configuration configuration) : ResourceBase(request, new ConfiguredResource(ResourcePath.Files, HttpMethod.Get)), IResponseProducer
{
    public async Task<string> ProduceResponseAsync()
    {
        Console.WriteLine($"Files Directory: {configuration.FilesDirectory}");
        if (string.IsNullOrEmpty(configuration.FilesDirectory))
            return await Task.FromResult(HttpResponseWithoutBody.Http404NotFoudResponse);

        var filePath = Path.Combine(configuration.FilesDirectory, IncommingRequestPathArgs[1]);
        Console.WriteLine($"Requested file: {filePath}");

        if (!File.Exists(filePath)) return await Task.FromResult(HttpResponseWithoutBody.Http404NotFoudResponse);

        Console.WriteLine($"Starting to read file: {filePath}");
        var bytes = await File.ReadAllBytesAsync(filePath);
        var filePlainTextContent = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

        Console.WriteLine($"file content: {filePlainTextContent}");
        Console.WriteLine($"Producing response for file: {filePlainTextContent}");

        return await Task.FromResult(new Response(
                    new StatusLine((int)HttpStatusCode.OK, nameof(HttpStatusCode.OK)),
                    new Header("application/octet-stream", bytes.Length.ToString()),
                    filePlainTextContent
                ).ToString());
    }
}
