﻿namespace Text_Based_Game.Classes
{
    static class TextHelper
    {
        /// <summary>
        /// 
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
        /// Prints a string one character at a time
        /// </summary>
        public static void PrintStringCharByChar(string line)
        {
            foreach (char c in line)
            {
                Console.Write(c);
                // for debug
                Thread.Sleep(5);
                //for build
                //Thread.Sleep(25);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ChangeForegroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}
