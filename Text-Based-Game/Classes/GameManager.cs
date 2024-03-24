namespace Text_Based_Game.Classes
{
    internal class GameManager
    {
        // PROPERTIES
        private Player Player { get; set; }
        public GamePath CurrentPath { get; set; }

        // VARIABLES
        public bool CanTakeFinalPath = false;
        private readonly int MaxPathLengthEasy = 15;
        private readonly int MinPathLengthEasy = 10;
        private readonly int MaxPathLengthMedium = 20;
        private readonly int MinPathLengthMedium = 10;
        private readonly int MaxPathLengthHard = 25;
        private readonly int MinPathLengthHard = 15;
        private readonly int MaxPathLengthFinal = 30;
        private readonly int MinPathLengthFinal = 20;
        private readonly string[] ReturnToTownMessages;
        private readonly string[] PathStartMessages;
        private readonly string[] PathCompletionMessages;
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
            //Console.WriteLine("TESTING NewGameModifier: " + Globals.NewGameModifier);
            CurrentPath.TraversePath();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartNewJourney()
        {
            Globals.NewGameModifier++;
            Player.WeaponInventory.Clear();
            Console.Clear();
            TextHelper.PrintTextFile(Globals.TitlePath, false);
            TextHelper.LineSpacing();
            Player.CurrentLocation = Location.Town;
            CurrentPath = GeneratePath(PathDifficulty.Easy);
            Player.SetCurrentHpToMax();
            StartGame();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SimulateRegularCombat(Enemy enemy)
        {
            TextHelper.PrintTextInColor($"\nYou've encountered {enemy.Name}, {enemy.CurrentHp} HP", ConsoleColor.Yellow);
            Thread.Sleep(500);
            do
            {
                if (Random.NextDouble() < enemy.DodgeChance)
                {
                    TextHelper.PrintTextInColor($"The {enemy.Name} gracefully evades your attack", ConsoleColor.DarkYellow);
                }
                else
                {
                    int[] playerAttack = Player.CalculateAttack();
                    enemy.TakeDamage(playerAttack[1]);
                    if (playerAttack[0] == 0)
                    {
                        TextHelper.PrintTextInColor($"You attack {enemy.Name} but trip and whiff entirely", ConsoleColor.Yellow);
                    }
                    else if (playerAttack[0] == 1)
                    {
                        TextHelper.PrintTextInColor($"You attack {enemy.Name} and do {playerAttack[1]} dmg, {enemy.CurrentHp}/{enemy.MaxHp} HP", ConsoleColor.Yellow);
                    }
                    else
                    {
                        TextHelper.PrintTextInColor($"You attack {enemy.Name} {playerAttack[0]} times for a total of {playerAttack[1]} dmg, {enemy.CurrentHp}/{enemy.MaxHp} HP", ConsoleColor.Yellow);
                    }
                }
                Thread.Sleep(500);

                if (enemy.CurrentHp > 0)
                {
                    int enemyDamage = enemy.CalculateAttack();
                    Player.TakeDamage(enemyDamage);
                    TextHelper.PrintTextInColor($"{enemy.Name} attacks, you take {enemyDamage} dmg, {Player.CurrentHp}/{Player.MaxHp} HP", ConsoleColor.DarkYellow);
                }
                Thread.Sleep(500);

            } while (enemy.CurrentHp > 0 && Player.CurrentHp > 0);

            if (enemy.CurrentHp <= 0)
            {
                CurrentPath.XpFromMobsOnPath += enemy.XpDropped;
                TextHelper.PrintTextInColor($"The {enemy.Name} collapses, ", ConsoleColor.Yellow, false);
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
        }

        /// <summary>
        /// 
        /// </summary>
        public void SimulateBossCombat(Boss boss)
        {
            TextHelper.PrintTextInColor($"\nYou've encountered {boss.Name}, {boss.CurrentHp} HP", ConsoleColor.Red);
            Thread.Sleep(500);
            do
            {
                if (Random.NextDouble() < boss.DodgeChance)
                {
                    TextHelper.PrintTextInColor($"{boss.Name} gracefully evades your attack", ConsoleColor.DarkRed);
                }
                else
                {
                    int[] playerAttack = Player.CalculateAttack();
                    if (playerAttack[0] == 0)
                    {
                        TextHelper.PrintTextInColor($"You attack {boss.Name} but trip and whiff entirely", ConsoleColor.Red);
                    }
                    else if (playerAttack[0] == 1)
                    {
                        TextHelper.PrintTextInColor($"You attack {boss.Name} and do {playerAttack[1]} dmg", ConsoleColor.Red);
                    }
                    else
                    {
                        TextHelper.PrintTextInColor($"You attack {boss.Name} {playerAttack[0]} times for a total of {playerAttack[1]} dmg", ConsoleColor.Red);
                    }
                    boss.TakeDamage(playerAttack[1]);
                }
                Thread.Sleep(500);

                if (boss.CurrentHp > 0)
                {
                    int enemyDamage = boss.CalculateAttack();
                    Player.TakeDamage(enemyDamage);
                    TextHelper.PrintTextInColor($"{boss.Name} attacks, you take {enemyDamage} dmg, {Player.CurrentHp}/{Player.MaxHp} HP", ConsoleColor.DarkRed);
                }
                Thread.Sleep(500);

            } while (boss.CurrentHp > 0 && Player.CurrentHp > 0);

            if (boss.CurrentHp <= 0)
            {
                CurrentPath.XpFromMobsOnPath += boss.XpDropped;
                TextHelper.PrintTextInColor($"{boss.Name} collapses, ", ConsoleColor.Red, false);
                if (boss.WeaponToDrop != null)
                {
                    TextHelper.PrintTextInColor($"you've gained {boss.XpDropped} XP", ConsoleColor.Blue, false);
                    TextHelper.PrintTextInColor($" and {boss.WeaponToDrop.Name} ({boss.WeaponToDrop.Rarity})!\n", ConsoleColor.Blue, false);
                    Player.PickUpWeapon(boss.WeaponToDrop);
                }
                else
                {
                    TextHelper.PrintTextInColor($"you've gained {boss.XpDropped} XP!\n", ConsoleColor.Blue, false);
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
                        Player.Heal((int)Player.MaxHp / 2);
                        Player.IsDead = false;
                        SimulateBossCombat(boss);
                    }
                    else
                    {
                        TextHelper.LineSpacing(0);
                        Player.IsDead = true;
                        CurrentPath.TeleportToTown(boss.Name);
                    }
                }
                else
                {
                    Player.IsDead = true;
                    CurrentPath.TeleportToTown(boss.Name);
                }
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
                TextHelper.PrintStringCharByChar(ReturnToTownMessages[Random.Next(ReturnToTownMessages.Length)], ConsoleColor.White);
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
