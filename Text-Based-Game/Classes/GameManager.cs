namespace Text_Based_Game.Classes
{
    internal class GameManager
    {
        const string returnToTownMessagesPath = "Content/returnToTownMessages.txt";
        const string pathStartMessagesPath = "Content/pathStartMessages.txt";
        const string pathCompletionMessagesPath = "Content/pathCompletionMessages.txt";

        Player Player { get; set; }
        GamePath? CurrentPath { get; set; }
        int MaxPathLength = 20;
        int MinPathLength = 10;
        bool IsGameRunning = true;
        string[] returnToTownMessages;
        string[] allPathStartMessages;
        string[] allPathCompletionMessages;

        // CONSTRUCTORS
        public GameManager()
        {
            Player = new(this);
            returnToTownMessages = File.ReadAllLines(returnToTownMessagesPath);
            allPathStartMessages = File.ReadAllLines(pathStartMessagesPath);
            allPathCompletionMessages = File.ReadAllLines(pathCompletionMessagesPath);
        }

        // METHODS

        /// <summary>
        /// 
        /// </summary>
        public void StartGame()
        {
            // instantiate Tutorial Path
            GeneratePath(PathDifficulty.Easy);
            CurrentPath?.Start();

            while (IsGameRunning)
            {
                ShowTownOptions();
                // to keep console open
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowTownOptions()
        {
            if (Player.CurrentLocation != Location.Town)
            {
                Random random = new();
                TextHelper.PrintStringCharByChar(returnToTownMessages[random.Next(returnToTownMessages.Length)]);
                Player.CurrentLocation = Location.Town;
                Player.SetCurrentHpToMax();
            }

            Console.Write("\nDo you want to start another (p)ath or see your (s)tats?: ");
            ConsoleKeyInfo key = Console.ReadKey();
            bool validInput = false;
            if (key.Key == ConsoleKey.S || key.Key == ConsoleKey.P) validInput = true;
            while (!validInput)
            {
                Console.Write("\nNo choice was made, please try again: ");
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.S || key.Key == ConsoleKey.P) validInput = true;

            }

            if (key.Key == ConsoleKey.S)
            {
                Player.ShowStats();
            }
            else if (key.Key == ConsoleKey.P)
            {
                Console.Write("\nDo you want to venture down an (e)asy, (m)edium or (h)ard path?: ");
                key = Console.ReadKey();
                validInput = false;
                if (key.Key == ConsoleKey.E || key.Key == ConsoleKey.M || key.Key == ConsoleKey.H) validInput = true;
                while (!validInput)
                {
                    Console.Write("\nNo choice was made, please try again: ");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.E || key.Key == ConsoleKey.M || key.Key == ConsoleKey.H) validInput = true;
                }

                switch (key.Key)
                {
                    case ConsoleKey.E:
                        GeneratePath(PathDifficulty.Easy);
                        break;
                    case ConsoleKey.M:
                        GeneratePath(PathDifficulty.Medium);
                        break;
                    case ConsoleKey.H:
                        GeneratePath(PathDifficulty.Hard);
                        break;
                }
                Console.WriteLine("\n");
                CurrentPath?.Start();
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GeneratePath(PathDifficulty chosenDifficulty)
        {
            Random random = new();

            string randomPathStartMessage = allPathStartMessages[random.Next(allPathStartMessages.Length)];
            string randomPathCompletionMessage = allPathCompletionMessages[random.Next(allPathCompletionMessages.Length)];

            int randomPathLength = random.Next(MinPathLength, MaxPathLength + 1);

            GamePath newPath = new(
                randomPathStartMessage,
                randomPathCompletionMessage,
                randomPathLength,
                Player,
                chosenDifficulty,
                this
                );

            CurrentPath = newPath;
        }
    }
}
