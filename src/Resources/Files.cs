using System.Net;
using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Models;
using codecrafters_http_server.src.Models.ResponseComponents;
using codecrafters_http_server.src.Utils;

namespace codecrafters_http_server.src.Resources;

public sealed class Files(IRequest request) : ResourceBase(request, new ConfiguredResource(ResourcePath.Files, HttpMethod.Get)), IResponseProducer
{
    public async Task<string> ProduceResponseAsync()
    {
        //"./codecrafters-http-server-csharp/tmp"
        var currentDir = Directory.GetCurrentDirectory();
        var tempDir = Path.Combine(currentDir, "tmp");

        Console.WriteLine($"Current Directory: {tempDir}");

        var existingTempDir = Directory.Exists(tempDir);
        if (!existingTempDir) return await Task.FromResult(HttpResponseWithoutBody.Http404NotFoudResponse);

        Console.WriteLine($"Searching for files in: {tempDir}");
        var files = Directory.GetFiles(tempDir, "*.*", SearchOption.AllDirectories);
        if (files.Length == 0) return await Task.FromResult(HttpResponseWithoutBody.Http404NotFoudResponse);


        Console.WriteLine($"Searching for specific file in: {tempDir}");
        var existingFile = files.FirstOrDefault(x => x.Contains(IncommingRequestPathArgs[1], StringComparison.OrdinalIgnoreCase));
        if (existingFile is null) return await Task.FromResult(HttpResponseWithoutBody.Http404NotFoudResponse);

        // var filePath = Path.Combine(tempDir, existingFile);
        Console.WriteLine($"FilePath: {existingFile}");

        var filePlainTextContent = await File.ReadAllTextAsync(existingFile);
        var bytes = await File.ReadAllBytesAsync(filePlainTextContent);

        Console.WriteLine($"file content: {filePlainTextContent}");
        Console.WriteLine($"Producing response for file: {filePlainTextContent}");

        return await Task.FromResult(new Response(
                    new StatusLine((int)HttpStatusCode.OK, nameof(HttpStatusCode.OK)),
                    new Header("application/octet-stream", bytes.Length.ToString()),
                    filePlainTextContent
                ).ToString());
    }
}
