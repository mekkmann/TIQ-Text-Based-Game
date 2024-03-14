using Text_Based_Game.Classes;

namespace Text_Based_Game
{
    internal class Program
    {
        const string TitlePath = "Content/title.txt";
        const string IntroPath = "Content/intro.txt";
        static void Main(string[] args)
        {
            // print the title
            PrintTextFile(TitlePath, false);
            // spacing
            Console.WriteLine();
            // for build
            // wait 2.5 seconds
            //Thread.Sleep(2500);
            // print the intro lore
            PrintTextFile(IntroPath, true);
            // spacing
            Console.WriteLine();



            Console.Write("Are you ready to start your adventure? (Y)es or any other key to quit: ");
            ConsoleKeyInfo key = Console.ReadKey();
            // if key does not equal 'Y'/'y', quit game
            if (key.Key != ConsoleKey.Y)
            {
                Environment.Exit(0);
            }

            // spacing
            Console.WriteLine();

            // start game loop


            // instantiate the player object
            Player player = new();

            // to keep console open
            Console.ReadLine();
        }

        /// <summary>
        /// Prints the contents of a text file, line by line. As fast as possible or letter by letter.
        /// </summary>
        /// <param name="path">path to file</param>
        /// <param name="letterByLetter">if the text should be printed "immediately" or letter by letter</param>
        static void PrintTextFile(string path, bool letterByLetter)
        {
            string[] fileLines = File.ReadAllLines(path);

            if (letterByLetter)
            {
                foreach (string line in fileLines)
                {
                    PrintStringCharByChar(line);
                    // for build
                    //Thread.Sleep(500);
                    Console.WriteLine();
                }
            }
            else
            {
                foreach (string line in fileLines)
                {
                    Console.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Prints a string one character at a time
        /// </summary>
        static void PrintStringCharByChar(string line)
        {
            foreach (char c in line)
            {
                Console.Write(c);
                //for build
                //Thread.Sleep(25);
            }
        }
    }
}
