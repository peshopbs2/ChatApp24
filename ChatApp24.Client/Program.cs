using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Reflection.Metadata;
string hostname = Dns.GetHostName();
IPHostEntry ipHostInfo = Dns.GetHostEntry(hostname);
IPAddress ip = ipHostInfo.AddressList[0];
IPEndPoint endpoint = new IPEndPoint(ip, 11223);
Socket socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
try
{
    socket.Connect(endpoint);
    Console.WriteLine($"Connected to {socket.RemoteEndPoint.ToString()}");
    byte[] buffer = new byte[4096];
    while (true)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        string input = Console.ReadLine();
        byte[] message = Encoding.UTF8.GetBytes(input);
        socket.Send(message);

        if(input == "exit")
        {
            break;
        }

        int messageSize = socket.Receive(buffer);
        string response = Encoding.UTF8.GetString(buffer, 0, messageSize);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(response);
    }

    socket.Shutdown(SocketShutdown.Both);
    socket.Close();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}