
using System.Net.Sockets;

namespace Server
{
    internal class Program
    {
        private static string GetUserInput(string description, string errorMessage, params Func<string, bool>[] validators)
        {
            string data;
            while (true)
            {
            NotFound:
                Console.Write(description);
                data = Console.ReadLine();
                data = data.Trim();

                foreach (var validator in validators)
                {
                    if (!validator(data))
                    {
                        Console.WriteLine(errorMessage);
                        goto NotFound;
                    }
                }
                break;
            }
            return data;
        }

        static void Main(string[] args)
        {
            GetUserInput("What is the server address: ", "", (data) =>
            {
                try
                {
                    string[] tokens = data.Split(':');
                    string ip = tokens[0];
                    int port = int.Parse(tokens[1]);
                    Server.Start(ip, port);
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex is SocketException)
                        Console.WriteLine("Server address is not valid");
                    else
                        Console.WriteLine("Invalid formatting");
                    return false;
                }
            });

            Console.WriteLine("Press any key if you want to shutdown the server...");
            Console.ReadKey();
            Server.Shutdown();
        }
    }
}