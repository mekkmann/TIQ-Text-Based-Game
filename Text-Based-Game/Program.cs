namespace Text_Based_Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PrintTitle();

            // to keep console open
            Console.ReadLine();
        }

        static void PrintTitle()
        {
            string[] fileLines = File.ReadAllLines("title.txt");

            foreach (string line in fileLines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
