using System.Net;
using System.Net.Sockets;
using codecrafters_http_server.src;
using codecrafters_http_server.src.Models;
using System.Text;
// You can use print statements as follows for debugging, they'll be visible when running tests.

string[] availableResources = ["/"];

Console.WriteLine("Logs from your program will appear here!");

TcpListener server = new TcpListener(IPAddress.Any, 4221);
server.Start();

while (true)
{
    var cancellationTokenTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
    var connectedSocket = await server.AcceptSocketAsync(cancellationTokenTimeout);

    try
    {
        var buffer = new byte[1024];
        var receivedBytes = await connectedSocket.ReceiveAsync(buffer, SocketFlags.None, cancellationTokenTimeout);
        var incommingRequestString = Encoding.UTF8.GetString(buffer, 0, receivedBytes);

        Console.WriteLine($"Received request: {incommingRequestString}");
        ArgumentNullException.ThrowIfNullOrEmpty(incommingRequestString);

        var requestLine = new RequestLine(incommingRequestString);
        var doesResourceExists = availableResources.Any(x => x.Equals(requestLine.Resource, StringComparison.OrdinalIgnoreCase));
        if (!doesResourceExists)
        {
            Console.WriteLine($"Resource not found: {requestLine.Resource}");
            await connectedSocket.SendAsync(Encoding.UTF8.GetBytes(HttpResponse.Http404NotFoudResponse));
        }

        var responseBytes = Encoding.UTF8.GetBytes(HttpResponse.Http200OkResponse);
        await connectedSocket.SendAsync(responseBytes);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex.Message}");
    }
    finally
    {
        await connectedSocket.DisconnectAsync(false);
    }
}