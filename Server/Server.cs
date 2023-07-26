using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal static class Server
    {
        private static TcpListener listener;
        private static readonly List<Client> clients = new List<Client>();
        private static bool isServerActive = true;

        private static readonly string title = "      ______    _ ___ __    __ _ ___       __    _ ___\r\n| ||   |  | |V||_| | |_    |_ |_) | |/    /  |_||_| | \r\n|_||__ | _|_| || | | |__   |__|  _|_|\\    \\__| || | | \r\n";

        public static void Start(string ip, int port)
        {
            listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();
            Console.WriteLine($"Server started on adress {ip}:{port}");
            StartAcceptingClients();
        }

        private static async void StartAcceptingClients()
        {
            while (isServerActive)
            {
                TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                Console.WriteLine($"Incoming connection from {tcpClient.Client.RemoteEndPoint}");
                
                Client client = new Client(tcpClient);
                string name = await client.Reader.ReadLineAsync();
                name = name.Split(":")[0];
                client.Name = name;
                BroadcastMessage($"{client.Name} has joined the chat");
                string welcomeMessage = "\n\nWELCOME TO THE\n\n" + title;
                welcomeMessage += "\n==========================================================";
                SendMessage(client, welcomeMessage);
                clients.Add(client);
                HandeClientMessages(client);
            }
        }

        private static async void HandeClientMessages(Client client)
        {
            try
            {
                while (isServerActive)
                {
                    string data = await client.Reader.ReadLineAsync();
                    Console.WriteLine(data);
                    BroadcastMessage(data);
                }
            }
            catch (Exception)
            {
                clients.Remove(client);
                Console.WriteLine($"Client {client._TcpClient.Client.RemoteEndPoint} disconnected");
                BroadcastMessage($"{client.Name} has left the chat");
            }
        }

        private static void BroadcastMessage(string message)
        {
            foreach (Client client in clients)
            {
                SendMessage(client, message);
            }
        }

        private static void SendMessage(Client client, string message)
        {
            client.Writer.WriteLine(message);
            client.Writer.Flush();
        }

        public static void Shutdown()
        {
            foreach (Client client in clients)
            {
                //send message to disconnect the client
                client._TcpClient.Close();
            }
            clients.Clear();
            isServerActive = false;
            listener.Stop();
        }
    }
}
