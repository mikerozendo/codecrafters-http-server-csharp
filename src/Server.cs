using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Utils;
using codecrafters_http_server.src.Models.RequestComponents;
using codecrafters_http_server.src.Resources;
using codecrafters_http_server.src;

Configuration configuration = new()
{
    FilesDirectory = args.Length > 0 ? args[1] : string.Empty
};

var serviceProvider = BuildServiceProvider(configuration);

TcpListener server = new(IPAddress.Any, 4221);

server.Start();

while (true)//trash, too many resources
    server.BeginAcceptTcpClient(OnConnectedClientCallBack, server);

void OnConnectedClientCallBack(IAsyncResult asyncResult)
{
    Task.Run(async () =>
    {
        Console.WriteLine($"Received connection request at: {DateTime.UtcNow}");

        var listener = (TcpListener)asyncResult.AsyncState!;
        using var client = listener.EndAcceptTcpClient(asyncResult);
        using var socket = client.GetStream().Socket;

        var serviceScope = serviceProvider.CreateAsyncScope(); // Each task is going to have its own scope, necessary once requests are going to be handled in parallel

        try
        {
            var scopedServiceProvider = serviceScope.ServiceProvider;
            var bufferToReceive = new byte[1024];

            var cancellationTokenTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token;
            var receivedBytes = await socket.ReceiveAsync(bufferToReceive, SocketFlags.None, cancellationTokenTimeout);

            var incommingRequestString = Encoding.UTF8.GetString(bufferToReceive, 0, receivedBytes);
            Console.WriteLine($"Received request: {incommingRequestString}");

            var request = scopedServiceProvider.GetRequiredService<IRequest>();
            request.BuildRequestComponents(incommingRequestString);//internally builds Request components from the raw http request string

            if (request.Components.Count == 0)
                throw new ArgumentNullException(nameof(request.Components), "Request components are not registered in the service provider.");

            var configuredEndpoints = scopedServiceProvider.GetRequiredService<IEnumerable<IResponseProducer>>();

            var existingResource = configuredEndpoints.SingleOrDefault(x => ((ResourceBase)x).HasMatchingRoute());
            if (existingResource is null)
            {
                Console.WriteLine("Resource not found");

                await socket.SendAsync(Encoding.UTF8.GetBytes(HttpResponseWithoutBody.Http404NotFoudResponse));

                Console.WriteLine("Response has been sent"); return;
            }

            var response = await existingResource.ProduceResponseAsync();
            Console.WriteLine($"Sending Response: {response}");

            await socket.SendAsync(
                Encoding.UTF8.GetBytes(response.ToCharArray()),
                SocketFlags.None,
                cancellationTokenTimeout
            );
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Timeout");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex}");
        }
        finally
        {
            Console.WriteLine("Freeing socket resources");
            await serviceScope.DisposeAsync();
            server.BeginAcceptTcpClient(OnConnectedClientCallBack, server);
            Console.WriteLine("Server is now listening for new connections");
        }
    });
}

ServiceProvider BuildServiceProvider(Configuration configuration = null)
{
    var services = new ServiceCollection();

    services.AddScoped<IRequestComponent, Line>();
    services.AddScoped<IRequestComponent, Header>();
    services.AddScoped<IRequest, Request>();

    services.AddScoped<IResponseProducer, Echo>();
    services.AddScoped<IResponseProducer, Root>();
    services.AddScoped<IResponseProducer, UserAgent>();
    services.AddScoped<IResponseProducer, Files>();

    if (configuration is not null)
        services.AddSingleton(configuration);

    var provider = services.BuildServiceProvider();

    return provider;
}

// for (int i = 0; i < args.Length; i++) // only used for tests at https://app.codecrafters.io/courses/http-server/stages/ap6
// {
//     var hasCreateDirArg = args[i].StartsWith("--directory");
//     if (hasCreateDirArg)
//     {
//         var path = args[i + 1];
//         Console.WriteLine($"Directory argument: {path}");
//         // configuration.FilesDirectory = path;
//         if (!Directory.Exists(path))
//         {
//             Directory.CreateDirectory(path);
//             Console.WriteLine($"Directory '{path}' has been created.");
//         }

//         break;//needs to change if a new command args is added
//     }
// }
