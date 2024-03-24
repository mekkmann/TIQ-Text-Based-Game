namespace Text_Based_Game.Classes
{
    static class TextHelper
    {
        public static void PrintDeathAnimation(ConsoleColor color)
        {
            var frame1 = File.ReadAllLines($"Content/death1.txt");
            var frame2 = File.ReadAllLines("Content/death2.txt");
            var frame3 = File.ReadAllLines("Content/death3.txt");
            var frame4 = File.ReadAllLines("Content/death4.txt");
            var frame5 = File.ReadAllLines("Content/death5.txt");
            var frame6 = File.ReadAllLines("Content/death6.txt");
            var frame7 = File.ReadAllLines("Content/death7.txt");

            Console.Clear();
            ChangeForegroundColor(color);
            foreach (var line in frame1)
            {
                Console.WriteLine(line);
            }
            Thread.Sleep(100);
            Console.Clear();
            foreach (var line in frame2)
            {
                Console.WriteLine(line);
            }
            Thread.Sleep(100);
            Console.Clear();
            foreach (var line in frame3)
            {
                Console.WriteLine(line);
            }
            Thread.Sleep(100);
            Console.Clear();
            foreach (var line in frame4)
            {
                Console.WriteLine(line);
            }
            Thread.Sleep(100);
            Console.Clear();
            foreach (var line in frame5)
            {
                Console.WriteLine(line);
            }
            Thread.Sleep(100);
            Console.Clear();
            foreach (var line in frame6)
            {
                Console.WriteLine(line);
            }
            Thread.Sleep(100);
            Console.Clear();
            foreach (var line in frame7)
            {
                Console.WriteLine(line);
            }

            ChangeForegroundColor(ConsoleColor.Gray);
        }
        /// <summary>
        /// Reads all lines in a file and either prints it line by line or letter by letter.
        /// </summary>
        public static void PrintTextFile(string path, bool letterByLetter)
        {
            string[] fileLines = File.ReadAllLines(path);

            if (letterByLetter)
            {
                //foreach (string line in fileLines)
                //{
                //    PrintStringCharByChar(line);
                //    // for build
                //    //Thread.Sleep(500);
                //    Console.WriteLine();
                //}
                foreach (string line in fileLines)
                {
                    Console.WriteLine(line);
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
        /// Prints a string one character at a time. Can write text in color specified.
        /// </summary>
        public static void PrintStringCharByChar(string line, ConsoleColor color = ConsoleColor.Gray)
        {
            ChangeForegroundColor(color);
            foreach (char c in line)
            {
                Console.Write(c);
                // for debug
                Thread.Sleep(5);
                //for build
                //Thread.Sleep(25);
            }
            ChangeForegroundColor(ConsoleColor.Gray);
        }

        /// <summary>
        /// Writes text in color specified to the console. Defaults to write on a new line, can be set to write on the same line.
        /// </summary>
        public static void PrintTextInColor(string line, ConsoleColor color, bool newLine = true)
        {
            ChangeForegroundColor(color);
            if (newLine)
            {
                Console.WriteLine(line);
            }
            else
            {
                Console.Write(line);
            }
            ChangeForegroundColor(ConsoleColor.Gray);
        }

        /// <summary>
        /// Changes the foreground (text) color of the console 
        /// </summary>
        public static void ChangeForegroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Writes a blank line to the console
        /// </summary>
        public static void LineSpacing(int lines = 1)
        {
            if (lines < 1)
            {
                Console.WriteLine();
            }
            else
            {
                string newLines = "";
                for (int i = 0; i < lines; i++)
                {
                    newLines += "\n";
                }
                Console.WriteLine(newLines);
            }
        }
    }
}
