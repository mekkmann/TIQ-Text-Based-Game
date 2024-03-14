namespace Text_Based_Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // print the title
            PrintTextFile("title.txt", false);
            // spacing
            Console.WriteLine();
            // print the intro lore
            PrintTextFile("intro.txt", true);

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
                //for testing
                Thread.Sleep(10);
                //for build
                //Thread.Sleep(25);
            }
        }
    }
}
