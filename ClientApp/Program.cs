
namespace ClientApp
{
    internal class Program
    {
        private static void DeletePrevConsoleLine()
        {
            if (Console.CursorTop == 0) return;
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
        }

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
                        DeletePrevConsoleLine();
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
            using Client client = new Client();

            string name = GetUserInput("Enter your name(leave blank for a random name): ", "A name must contain at least 3 characters!", (data) => data.Length >= 3 || data.Length == 0);
            if (name.Length > 0)
                client.SetName(name);

            string serverAddress = GetUserInput("Server Address: ", "Either the format or the address is invalid!", (data) =>
            {
                try
                {
                    string[] tokens = data.Split(":");
                    string ip = tokens[0];
                    int port = int.Parse(tokens[1]);
                    return client.Connnect(ip, port);
                }
                catch (Exception)
                {
                    return false;
                }
            });

            while (client.Connected)
            {
                string line = Console.ReadLine();
                DeletePrevConsoleLine();
                if (string.IsNullOrEmpty(line)) continue;
                if (line[0] == 24)
                    break;
                client.SendMessage(line);
            }
        }
    }
}