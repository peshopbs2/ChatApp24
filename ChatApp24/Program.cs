using System.Net;
using System.Net.Sockets;
using System.Text;

string hostname = Dns.GetHostName();
IPHostEntry ipHostInfo = Dns.GetHostEntry(hostname);
IPAddress ip = ipHostInfo.AddressList[0];
IPEndPoint endpoint = new IPEndPoint(ip, 11223);

Socket socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

try
{
    socket.Bind(endpoint);
    socket.Listen();
    Console.WriteLine("Listening for the voices...");
    Socket handle = socket.Accept();
    byte[] buffer = new byte[4096];
    string message = "";
    while (true)
    {
        int messageSize = handle.Receive(buffer);
        message = Encoding.UTF8.GetString(buffer, 0, messageSize);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(message);
        if (message == "exit")
        {
            break;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        string input = Console.ReadLine();
        byte[] data = Encoding.UTF8.GetBytes(input);
        handle.Send(data);
    }

    handle.Shutdown(SocketShutdown.Both);
    handle.Close();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine("Something went wrong...");
}