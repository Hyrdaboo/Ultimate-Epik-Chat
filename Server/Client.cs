using System.Net.Sockets;

namespace Server
{
    internal class Client : IDisposable
    {
        public TcpClient _TcpClient { get; private set; }
        private readonly NetworkStream stream;
        public StreamReader Reader { get; private set; }
        public StreamWriter Writer { get; private set; }
        public string Name = "Someone";

        public Client(TcpClient client)
        {
            _TcpClient = client;
            stream = _TcpClient.GetStream();
            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream);
        }

        public void Dispose()
        {
            stream.Dispose();
            Reader.Dispose();
            Writer.Dispose();
            _TcpClient.Dispose();
        }
    }
}
