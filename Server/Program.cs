
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
            GetUserInput("What port should the server listen on: ", "", (data) =>
            {
                try
                {
                    int port = int.Parse(data);
                    Server.Start(port);
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