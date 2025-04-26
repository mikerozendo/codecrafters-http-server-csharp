using System.Net;
using System.Net.Sockets;
using codecrafters_http_server.src.Models;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

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

                var cancellationTokenTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
                var receivedBytes = await socket.ReceiveAsync(bufferToReceive, SocketFlags.None, cancellationTokenTimeout);

                var incommingRequestString = Encoding.UTF8.GetString(bufferToReceive, 0, receivedBytes);

                Console.WriteLine($"Received request: {incommingRequestString}");
                ArgumentNullException.ThrowIfNullOrEmpty(incommingRequestString);

                var requestComponentsBuilder = scopedServiceProvider.GetRequiredService<IRequestComponentsBuilder>();
                ArgumentNullException.ThrowIfNull(requestComponentsBuilder, "Request components builder is not registered in the service provider.");

                var requestComponents = requestComponentsBuilder.BuildRequestComponents(incommingRequestString);
                if (requestComponents == Enumerable.Empty<IRequestComponent>())
                    throw new ArgumentNullException(nameof(requestComponents), "Request components are not registered in the service provider.");

                var requestLine = requestComponents.OfType<RequestLine>().Single();

                var resourceResolver = scopedServiceProvider.GetRequiredService<IResourceResolver>();
                ArgumentNullException.ThrowIfNull(resourceResolver, "Resource resolver is not registered in the service provider.");

                var existingResource = resourceResolver.ResolveResource(requestLine.Resource, requestLine.HttpMethod);
                if (existingResource is null)
                {
                    Console.WriteLine("Resource not found");

                    await socket.SendAsync(Encoding.UTF8.GetBytes(HttpResponseWithoutBody.Http404NotFoudResponse));

                    Console.WriteLine("Response has been sent"); return;
                }

                Console.WriteLine($"Resource: {existingResource.Path}");
                Console.WriteLine($"Sending response: {existingResource.Response}");

                var responseBuffer = Encoding.UTF8.GetBytes(existingResource.Response.ToString()!);

                await socket.SendAsync(responseBuffer); return;
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
    services.AddScoped<IRequestComponent, RequestLine>();
    services.AddScoped<IRequestComponent, RequestHeader>();
    services.AddScoped<IRequest, Request>();
    services.AddScoped<IRequestComponentsBuilder, RequestComponentsBuilder>();
    services.AddScoped<IResourceResolver, ResourceResolver>();
    var provider = services.BuildServiceProvider();
    return provider;
}
