using System.Net;
using System.Net.Sockets;

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

TcpListener server = new TcpListener(IPAddress.Any, 4221);
server.Start();

while (true)
{
    var connectedSocked = await server.AcceptSocketAsync();


    try
    {
        var response = "HTTP/1.1 200 OK\r\n\r\n";
        var responseBytes = System.Text.Encoding.UTF8.GetBytes(response);
        await connectedSocked.SendAsync(responseBytes);
    }
    finally
    {
        await connectedSocked.DisconnectAsync(false);
    }
}

