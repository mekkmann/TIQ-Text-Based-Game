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
        string[] ReturnToTownMessages;
        string[] PathStartMessages;
        string[] PathCompletionMessages;

        // CONSTRUCTORS
        public GameManager()
        {
            Player = new(this);
            ReturnToTownMessages = File.ReadAllLines(returnToTownMessagesPath);
            PathStartMessages = File.ReadAllLines(pathStartMessagesPath);
            PathCompletionMessages = File.ReadAllLines(pathCompletionMessagesPath);
        }

        // METHODS

        /// <summary>
        /// 
        /// </summary>
        public void StartGame()
        {
            // instantiate Tutorial Path
            CurrentPath = GeneratePath(PathDifficulty.Easy);
            CurrentPath?.Start();
        }

        public void SimulateCombat(Enemy enemy)
        {
            Random random = new();

            Console.WriteLine($"You've encountered {enemy.Name}");
            Thread.Sleep(500);
            do
            {
                if (random.NextDouble() < enemy.DodgeChance)
                {
                    Console.WriteLine($"The {enemy.Name} gracefully evades your attack");
                }
                else
                {
                    int playerDamage = random.Next(5, 11);
                    Console.WriteLine($"You attack {enemy.Name} and do {playerDamage} dmg");
                    enemy.HP -= playerDamage;
                }
                Thread.Sleep(500);



                if (enemy.HP > 0)
                {
                    int enemyDamage = random.Next(enemy.MinDamage, enemy.MaxDamage + 1);
                    Player.TakeDamage(enemyDamage);
                    Console.WriteLine($"{enemy.Name} attacks, you take {enemyDamage} dmg, {Player.CurrentHp} HP left");
                }
                Thread.Sleep(500);

            } while (enemy.HP > 0 && Player.CurrentHp > 0);

            if (enemy.HP <= 0)
            {
                CurrentPath.XpFromMobsOnPath += enemy.XpDropped;
                Console.WriteLine($"The {enemy.Name} collapses, you've gained {enemy.XpDropped} XP!\n");

            }
            else
            {
                Player.IsDead = true;
                CurrentPath?.TeleportToTown();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void HandleInputBuffering()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
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
                TextHelper.PrintStringCharByChar(ReturnToTownMessages[random.Next(ReturnToTownMessages.Length)]);
                Player.CurrentLocation = Location.Town;
                Player.SetCurrentHpToMax();
            }

            HandleInputBuffering();

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
        public GamePath GeneratePath(PathDifficulty chosenDifficulty)
        {
            Random random = new();

            string randomPathStartMessage = PathStartMessages[random.Next(PathStartMessages.Length)];
            string randomPathCompletionMessage = PathCompletionMessages[random.Next(PathCompletionMessages.Length)];

            int randomPathLength = random.Next(MinPathLength, MaxPathLength + 1);

            GamePath newPath = new(
                randomPathStartMessage,
                randomPathCompletionMessage,
                randomPathLength,
                Player,
                chosenDifficulty,
                this
                );

            return newPath;
        }
    }
}
