namespace Text_Based_Game.Classes
{
    internal class GameManager
    {
        Player Player { get; set; }
        public bool CanTakeFinalPath = false;
        public GamePath CurrentPath { get; set; }
        private int MaxPathLengthEasy = 15;
        private int MinPathLengthEasy = 10;
        private int MaxPathLengthMedium = 20;
        private int MinPathLengthMedium = 10;
        private int MaxPathLengthHard = 25;
        private int MinPathLengthHard = 15;
        private int MaxPathLengthFinal = 30;
        private int MinPathLengthFinal = 20;
        private string[] ReturnToTownMessages;
        private string[] PathStartMessages;
        private string[] PathCompletionMessages;
        public Random Random { get; set; }

        // CONSTRUCTORS
        public GameManager()
        {
            Random = new();
            Player = new(this);
            ReturnToTownMessages = File.ReadAllLines(Globals.ReturnToTownMessagesPath);
            PathStartMessages = File.ReadAllLines(Globals.PathStartMessagesPath);
            PathCompletionMessages = File.ReadAllLines(Globals.PathCompletionMessagesPath);
            CurrentPath = GeneratePath(PathDifficulty.Easy);
        }

        // METHODS

        /// <summary>
        /// Start the first path
        /// </summary>
        public void StartGame()
        {
            Console.WriteLine("TESTING NewGameModifier: " + Globals.NewGameModifier);
            CurrentPath.TraversePath();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartNewJourney()
        {
            Globals.NewGameModifier++;
            Player.WeaponsInBag.Clear();
            Console.Clear();
            TextHelper.PrintTextFile(Globals.TitlePath, false);
            StartGame();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SimulateRegularCombat(Enemy enemy)
        {
            TextHelper.ChangeForegroundColor(ConsoleColor.Yellow);
            Console.WriteLine($"\nYou've encountered {enemy.Name}, {enemy.Hp} HP");
            Thread.Sleep(500);
            do
            {
                if (Random.NextDouble() < enemy.DodgeChance)
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
                    enemy.TakeDamage(playerAttack[1]);
                }
                Thread.Sleep(500);

                if (enemy.Hp > 0)
                {
                    int enemyDamage = enemy.CalculateAttack();
                    Player.TakeDamage(enemyDamage);
                    Console.WriteLine($"{enemy.Name} attacks, you take {enemyDamage} dmg, {Player.CurrentHp}/{Player.MaxHp} HP");
                }
                Thread.Sleep(500);

            } while (enemy.Hp > 0 && Player.CurrentHp > 0);

            if (enemy.Hp <= 0)
            {
                CurrentPath.XpFromMobsOnPath += enemy.XpDropped;
                Console.Write($"The {enemy.Name} collapses, ");
                if (enemy.WeaponToDrop != null)
                {
                    TextHelper.PrintTextInColor($"you've gained {enemy.XpDropped} XP", ConsoleColor.Blue, false);
                    TextHelper.PrintTextInColor($" and {enemy.WeaponToDrop.Name} ({enemy.WeaponToDrop.Rarity})!\n\n", ConsoleColor.Blue, false);
                    Player.PickUpWeapon(enemy.WeaponToDrop);
                }
                else
                {
                    TextHelper.PrintTextInColor($"you've gained {enemy.XpDropped} XP!\n\n", ConsoleColor.Blue, false);
                }
            }
            else
            {
                Player.IsDead = true;
                CurrentPath.TeleportToTown(enemy.Name);
            }
            TextHelper.ChangeForegroundColor(ConsoleColor.Gray);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SimulateBossCombat(Boss boss)
        {
            TextHelper.ChangeForegroundColor(ConsoleColor.Red);
            Console.WriteLine($"\nYou've encountered {boss.Name}, {boss.Hp} HP");
            Thread.Sleep(500);
            do
            {
                if (Random.NextDouble() < boss.DodgeChance)
                {
                    Console.WriteLine($"{boss.Name} gracefully evades your attack");
                }
                else
                {
                    int[] playerAttack = Player.CalculateAttack();
                    if (playerAttack[0] == 0)
                    {
                        Console.WriteLine($"You attack {boss.Name} but trip and whiff entirely");
                    }
                    else if (playerAttack[0] == 1)
                    {
                        Console.WriteLine($"You attack {boss.Name} and do {playerAttack[1]} dmg");
                    }
                    else
                    {
                        Console.WriteLine($"You attack {boss.Name} {playerAttack[0]} times for a total of {playerAttack[1]} dmg");
                    }
                    boss.TakeDamage(playerAttack[1]);
                }
                Thread.Sleep(500);

                if (boss.Hp > 0)
                {
                    int enemyDamage = boss.CalculateAttack();
                    Player.TakeDamage(enemyDamage);
                    Console.WriteLine($"{boss.Name} attacks, you take {enemyDamage} dmg, {Player.CurrentHp}/{Player.MaxHp} HP");
                }
                Thread.Sleep(500);

            } while (boss.Hp > 0 && Player.CurrentHp > 0);

            if (boss.Hp <= 0)
            {
                CurrentPath.XpFromMobsOnPath += boss.XpDropped;
                Console.Write($"{boss.Name} collapses, ");
                if (boss.WeaponToDrop != null)
                {
                    TextHelper.PrintTextInColor($"you've gained {boss.XpDropped} XP", ConsoleColor.Blue, false);
                    TextHelper.PrintTextInColor($" and {boss.WeaponToDrop.Name} ({boss.WeaponToDrop.Rarity})!\n\n", ConsoleColor.Blue, false);
                    Player.PickUpWeapon(boss.WeaponToDrop);
                }
                else
                {
                    TextHelper.PrintTextInColor($"you've gained {boss.XpDropped} XP!\n\n", ConsoleColor.Blue, false);
                }
            }
            else
            {
                if (Player.Respawns > 0)
                {
                    Console.Write($"Would you like to (r)espawn ({Player.Respawns} / 3) or (t)eleport to town?: ");
                    ConsoleKeyInfo key = Console.ReadKey();
                    bool isValidInput = false;
                    if (key.Key == ConsoleKey.R || key.Key == ConsoleKey.T) isValidInput = true;
                    while (!isValidInput)
                    {
                        Console.Write("\ntry again: ");
                        key = Console.ReadKey();
                        if (key.Key == ConsoleKey.R || key.Key == ConsoleKey.T) isValidInput = true;
                    }

                    if (key.Key == ConsoleKey.R)
                    {
                        Player.Respawns--;
                        SimulateBossCombat(boss);
                    }
                    else
                    {
                        TextHelper.LineSpacing(0);
                    }
                }

                Player.IsDead = true;
                CurrentPath.TeleportToTown(boss.Name);
            }
            TextHelper.ChangeForegroundColor(ConsoleColor.Gray);
        }

        /// <summary>
        /// Handles input buffering
        /// </summary>
        public static void HandleInputBuffering()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
            }
        }

        public void ChoosePath()
        {
            if (!CanTakeFinalPath)
            {
                Console.Write("\nDo you want to venture down the (e)asy, (m)edium or (h)ard path?: ");
                ConsoleKeyInfo key = Console.ReadKey();
                bool validInput = false;
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
                ConsoleKeyInfo key = Console.ReadKey();
                bool validInput = false;
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
            CurrentPath.TraversePath();
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
                ChoosePath();
                return;
            }
        }

        /// <summary>
        /// Returns a GamePath based of the difficulty
        /// </summary>
        public GamePath GeneratePath(PathDifficulty chosenDifficulty)
        {
            string randomPathStartMessage = PathStartMessages[Random.Next(PathStartMessages.Length)];
            string randomPathCompletionMessage = PathCompletionMessages[Random.Next(PathCompletionMessages.Length)];

            int minLength = MinPathLengthEasy;
            int maxLength = MaxPathLengthEasy;
            switch (chosenDifficulty)
            {
                case PathDifficulty.Medium:
                    minLength = MinPathLengthMedium;
                    maxLength = MaxPathLengthMedium;
                    break;
                case PathDifficulty.Hard:
                    minLength = MinPathLengthHard;
                    maxLength = MaxPathLengthHard;
                    break;
                case PathDifficulty.Final:
                    minLength = MinPathLengthFinal;
                    maxLength = MaxPathLengthFinal;
                    break;
            }
            int randomPathLength = Random.Next(minLength, maxLength + 1);

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
