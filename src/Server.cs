using System.Net;
using System.Net.Sockets;
using codecrafters_http_server.src.Models;
using System.Text;

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
            try
            {
                var bufferToReceive = new byte[1024];

                var cancellationTokenTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
                var receivedBytes = await socket.ReceiveAsync(bufferToReceive, SocketFlags.None, cancellationTokenTimeout);

                var incommingRequestString = Encoding.UTF8.GetString(bufferToReceive, 0, receivedBytes);

                Console.WriteLine($"Received request: {incommingRequestString}");
                ArgumentNullException.ThrowIfNullOrEmpty(incommingRequestString);

                var requestLine = new RequestLine(incommingRequestString);
                var existingResource = ResourceResolver.ResolveResource(requestLine.Resource, requestLine.HttpMethod);

                if (existingResource is null)
                {
                    Console.WriteLine("Resource not found");
                    await socket.SendAsync(Encoding.UTF8.GetBytes(HttpResponseWithoutBody.Http404NotFoudResponse));
                }

                Console.WriteLine($"Resource: {existingResource!.Path}");
                Console.WriteLine($"Response: {existingResource.Response.ToString()}");
                var responseBuffer = Encoding.UTF8.GetBytes(existingResource.Response.ToString());
                await socket.SendAsync(responseBuffer);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Timeout");
            }
            finally
            {
                socket?.Dispose();
                server.BeginAcceptSocket(new AsyncCallback(AcceptCallback), server);
            }
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in AcceptCallback: {ex.Message}");
    }
}