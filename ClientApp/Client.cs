using NullLib.ConsoleEx;
using System.Net.Sockets;

namespace ClientApp
{
    internal class Client : IDisposable
    {
        private readonly TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        public bool Connected { get; private set; }
        private string Name;

        public Client()
        {
            client = new TcpClient();
            GenerateRandomName();
        }

        public bool Connnect(string ip, int port)
        {
            try
            {
                client.Connect(ip, port);
                stream = client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);
                Connected = true;
                SendMessage("");

                HandleMessagesFromServer();
            }
            catch (Exception)
            {
                Console.WriteLine($"Couldn't connect to {ip}:{port}\n");
            }
            return Connected;
        }

        private async void HandleMessagesFromServer()
        {
            while (Connected)
            {
                try
                {
                    string msg = await reader.ReadLineAsync();
                    string name = msg.Split(": ")[0];
                    if (name == Name)
                    {
                        int index = name.Length + 2;
                        msg = "You: " + msg.Substring(index, msg.Length - index);
                    }
                    ConsoleSc.WriteLine(msg);
                }
                catch (Exception)
                {
                    Console.WriteLine("Connection to server lost! You can quit the application.");
                    Disconnect();
                }
            }
        }

        public void SendMessage(string message)
        {
            writer.WriteLine(Name + ": " + message);
            writer.Flush();
        }

        public void Disconnect()
        {
            Connected = false;
            client.Close();
        }

        public void Dispose()
        {
            if (!Connected) return;
            stream.Dispose();
            reader.Dispose();
            writer.Dispose();
            client.Dispose();
        }

        public void SetName(string name)
        {
            Name = name.Trim();
        }

        private void GenerateRandomName()
        {
            string username = UsernameGenerator.Generate();
            if (username != null)
            {
                Name = username;
                return;
            }

            Random random = new Random();
            string name = "InvalidUser";
            for (int i = 0; i < 4; i++)
            {
                name += random.Next(0, 10);
            }

            Name = name;
        }
    }
}
