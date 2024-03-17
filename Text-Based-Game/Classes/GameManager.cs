namespace Text_Based_Game.Classes
{
    internal class GameManager
    {
        const string returnToTownMessagesPath = "Content/returnToTownMessages.txt";
        const string pathStartMessagesPath = "Content/pathStartMessages.txt";
        const string pathCompletionMessagesPath = "Content/pathCompletionMessages.txt";

        Player Player { get; set; }
        public bool CanTakeFinalPath = false;
        public GamePath? CurrentPath { get; set; }
        private int MaxPathLength = 20;
        private int MinPathLength = 10;
        private string[] ReturnToTownMessages;
        private string[] PathStartMessages;
        private string[] PathCompletionMessages;

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
            TextHelper.ChangeForegroundColor(ConsoleColor.Yellow);
            Console.WriteLine($"\nYou've encountered {enemy.Name}");
            Thread.Sleep(500);
            do
            {
                if (random.NextDouble() < enemy.DodgeChance)
                {
                    Console.WriteLine($"The {enemy.Name} gracefully evades your attack");
                }
                else
                {
                    int[] playerAttack = Player.CalculateAttack();
                    if (playerAttack[0] == 0)
                    {
                        Console.WriteLine($"You attack {enemy.Name} but trip and whiff entirely");
                    }
                    else if (playerAttack[0] == 1)
                    {
                        Console.WriteLine($"You attack {enemy.Name} and do {playerAttack[1]} dmg");
                    }
                    else
                    {
                        Console.WriteLine($"You attack {enemy.Name} {playerAttack[0]} times for a total of {playerAttack[1]} dmg");
                    }
                    enemy.HP -= playerAttack[1];
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
                Console.Write($"The {enemy.Name} collapses, ");
                TextHelper.PrintTextInColor($"you've gained {enemy.XpDropped} XP!\n\n", ConsoleColor.Blue, false);
            }
            else
            {
                Player.IsDead = true;
                CurrentPath?.TeleportToTown(enemy.Name);
            }
            TextHelper.ChangeForegroundColor(ConsoleColor.Gray);
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
            Player.IsDead = false;
            if (Player.CurrentLocation != Location.Town)
            {
                Random random = new();
                TextHelper.PrintStringCharByChar(ReturnToTownMessages[random.Next(ReturnToTownMessages.Length)], ConsoleColor.White);
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
                if (!CanTakeFinalPath)
                {
                    Console.Write("\nDo you want to venture down the (e)asy, (m)edium or (h)ard path?: ");
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
                            CurrentPath = GeneratePath(PathDifficulty.Easy);
                            break;
                        case ConsoleKey.M:
                            CurrentPath = GeneratePath(PathDifficulty.Medium);
                            break;
                        case ConsoleKey.H:
                            CurrentPath = GeneratePath(PathDifficulty.Hard);
                            break;
                    }
                }
                else
                {
                    Console.Write("\nDo you want to venture down the (e)asy, (m)edium, (h)ard or (f)inal path?: ");
                    key = Console.ReadKey();
                    validInput = false;
                    if (key.Key == ConsoleKey.E || key.Key == ConsoleKey.M || key.Key == ConsoleKey.H || key.Key == ConsoleKey.F) validInput = true;
                    while (!validInput)
                    {
                        Console.Write("\nNo choice was made, please try again: ");
                        key = Console.ReadKey();
                        if (key.Key == ConsoleKey.E || key.Key == ConsoleKey.M || key.Key == ConsoleKey.H || key.Key == ConsoleKey.F) validInput = true;
                    }

                    switch (key.Key)
                    {
                        case ConsoleKey.E:
                            CurrentPath = GeneratePath(PathDifficulty.Easy);
                            break;
                        case ConsoleKey.M:
                            CurrentPath = GeneratePath(PathDifficulty.Medium);
                            break;
                        case ConsoleKey.H:
                            CurrentPath = GeneratePath(PathDifficulty.Hard);
                            break;
                        case ConsoleKey.F:
                            CurrentPath = GeneratePath(PathDifficulty.Final);
                            break;
                    }
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
