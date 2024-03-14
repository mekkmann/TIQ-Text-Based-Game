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
            TextHelper.PrintTextFile(TitlePath, false);
            // spacing
            Console.WriteLine();
            // for build
            // wait 2.5 seconds
            //Thread.Sleep(2500);
            // print the intro lore
            TextHelper.PrintTextFile(IntroPath, true);
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
            Console.WriteLine("\n");

            // start game loop


            // instantiate the player object
            Player player = new();
            // instantiate Tutorial Path
            GamePath tutorialPath = new("Tutorial Path", 7, player, PathDifficulty.Easy);
            tutorialPath.Start();

            // to keep console open
            Console.ReadLine();
        }
    }
}
