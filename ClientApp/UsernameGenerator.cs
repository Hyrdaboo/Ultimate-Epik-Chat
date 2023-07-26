
namespace ClientApp
{
    internal static class UsernameGenerator
    {
        public static string Generate()
        {
            string username;
            try
            {
                var nouns = File.ReadLines("res/english-nouns.txt");
                var adjectives = File.ReadLines("res/english-adjectives.txt");

                Random rand = new Random();
                string noun = nouns.Skip(rand.Next(0, nouns.Count())).FirstOrDefault();
                string adjective = adjectives.Skip(rand.Next(0, adjectives.Count())).FirstOrDefault();
                noun = char.ToUpperInvariant(noun[0]) + noun[1..];
                adjective = char.ToUpperInvariant(adjective[0]) + adjective[1..];

                string numbers = "";
                for (int i = 0; i < 4; i++)
                {
                    numbers += rand.Next(0, 10);
                }
                username = adjective + noun + numbers;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Necessary files have been deleted!");
                return null;
            }
            catch (IOException)
            {
                Console.WriteLine("Unkown error occured while trying to generate a username");
                return null;
            }

            return username;
        }
    }
}
