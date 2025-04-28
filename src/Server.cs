using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Utils;
using codecrafters_http_server.src.Models.RequestComponents;
using codecrafters_http_server.src.Models.Resources;

var serviceProvider = BuildServiceProvider();

TcpListener server = new(IPAddress.Any, 4221);

server.Start();
server.BeginAcceptSocket(AcceptCallback, server);

await Task.Delay(Timeout.Infinite);

void AcceptCallback(IAsyncResult asyncResult)
{
    try
    {
        var server = (TcpListener)asyncResult.AsyncState!;
        var socket = server.EndAcceptSocket(asyncResult);

        Task.Run(async () =>
        {
            var serviceScope = serviceProvider.CreateAsyncScope(); // Each task is going to have its own scope, necessary once requests are going to be handled in parallel

            try
            {
                var scopedServiceProvider = serviceScope.ServiceProvider;
                var bufferToReceive = new byte[1024];

                var cancellationTokenTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(10000)).Token;
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

                var response = existingResource.ProduceResponse();
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
                Console.WriteLine($"General error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Freeing socket resources");

                socket.Dispose();
                await serviceScope.DisposeAsync();

                server.BeginAcceptSocket(new AsyncCallback(AcceptCallback), server);
                Console.WriteLine("Server is now listening for new connections");
            }
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in AcceptCallback: {ex.Message}");
    }
}


ServiceProvider BuildServiceProvider()
{
    var services = new ServiceCollection();

    services.AddScoped<IRequestComponent, Line>();
    services.AddScoped<IRequestComponent, Header>();
    services.AddScoped<IRequest, Request>();

    services.AddScoped<IResponseProducer, Echo>();
    services.AddScoped<IResponseProducer, Root>();
    var provider = services.BuildServiceProvider();
    return provider;
}
