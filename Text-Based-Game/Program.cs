using Text_Based_Game.Classes;

namespace Text_Based_Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // print the title
            TextHelper.PrintTextFile(Globals.TitlePath, false);
            // spacing
            Console.WriteLine();
            // for build
            // wait 2.5 seconds
            //Thread.Sleep(2500);
            // print the intro lore
            TextHelper.PrintTextFile(Globals.IntroPath, true);
            // spacing
            Console.WriteLine();
            // ask the player if they want to start the game
            Console.Write("Are you ready to start your adventure? (Y)es or any other key to quit: ");
            // get input
            ConsoleKeyInfo key = Console.ReadKey();
            // if input does not equal 'Y'/'y', quit game
            if (key.Key != ConsoleKey.Y)
            {
                Environment.Exit(0);
            }
            // spacing
            Console.WriteLine("\n");
            // initialize GameManager
            GameManager gameManager = new();
            // start game
            gameManager.StartGame();
        }
    }
}
