namespace Text_Based_Game.Classes
{
    enum PathDifficulty
    {
        Easy,
        Medium,
        Hard,
        Final
    }

    enum PathStepType
    {
        Walking,
        PlayerTalk,
        BossFight,
        MobFight
    }
    internal class GamePath
    {
        public GameManager GameManagerRef { get; private set; }
        public string PathStartMessage { get; set; }
        public string PathCompletionMessage { get; set; }
        public PathDifficulty Difficulty { get; set; }
        public int PathLength { get; set; }
        public List<PathStepType> PathSteps { get; set; }
        public bool IsCompleted { get; set; }
        public Player PlayerRef { get; set; }
        public float XpOnCompletion { get; set; }
        public float XpFromMobsOnPath { get; set; }

        // CONSTRUCTORS
        public GamePath(string pathStartMessage, string pathCompletionMessage, int pathLength, Player player, PathDifficulty difficulty, GameManager gameManager)
        {
            GameManagerRef = gameManager;
            PathStartMessage = pathStartMessage;
            PathCompletionMessage = pathCompletionMessage;

            Difficulty = difficulty;
            PathLength = 5;
            //PathLength = pathLength;
            PathSteps = MakePath(Difficulty);
            IsCompleted = false;
            PlayerRef = player;
            switch (Difficulty)
            {
                case PathDifficulty.Easy:
                    XpOnCompletion = 100f;
                    break;
                case PathDifficulty.Medium:
                    XpOnCompletion = 200f;
                    break;
                case PathDifficulty.Hard:
                    XpOnCompletion = 300f;
                    break;
                case PathDifficulty.Final:
                    XpOnCompletion = 1000f;
                    break;
            }
            XpFromMobsOnPath = 0;
        }

        // METHODS

        private List<PathStepType> MakePath(PathDifficulty difficulty)
        {
            List<PathStepType> steps = [];

            switch (difficulty)
            {
                case PathDifficulty.Easy:
                    for (int i = 0; i <= PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.MobFight);
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.Walking);
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.PlayerTalk);
                    }
                    steps.Add(PathStepType.BossFight);
                    break;
                case PathDifficulty.Medium:
                    for (int i = 0; i <= PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.MobFight);
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.Walking);
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.PlayerTalk);
                    }
                    steps.Add(PathStepType.BossFight);
                    break;
                case PathDifficulty.Hard:
                    for (int i = 0; i <= PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.MobFight);
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.Walking);
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.PlayerTalk);
                    }
                    steps.Add(PathStepType.BossFight);
                    break;
                case PathDifficulty.Final:
                    for (int i = 0; i <= PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.BossFight);
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.Walking);
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(PathStepType.PlayerTalk);
                    }
                    steps.Add(PathStepType.BossFight);
                    break;
            }

            return ShufflePath(steps);
        }


        /// <summary>
        /// 
        /// </summary>
        public void TraversePath()
        {
            PlayerRef.CurrentLocation = Location.Path;
            TextHelper.PrintStringCharByChar(PathStartMessage, ConsoleColor.White);
            TextHelper.LineSpacing(0);
            Random random = new();
            Thread.Sleep(random.Next(500, 1500));
            for (int i = 0; i < PathSteps.Count; i++)
            {
                switch (PathSteps[i])
                {
                    case PathStepType.Walking:
                        TextHelper.LineSpacing(0);
                        TextHelper.PrintTextInColor("*walking*", ConsoleColor.DarkGray);
                        break;
                    case PathStepType.PlayerTalk:
                        TextHelper.LineSpacing(0);
                        PlayerRef.SpeakAboutEnvironment();
                        break;
                    case PathStepType.MobFight:
                        Enemy currentEnemy = new(Difficulty);
                        GameManagerRef.SimulateRegularCombat(currentEnemy);
                        ShowOptionsAfterInteractiveEvent();
                        break;
                    case PathStepType.BossFight:
                        Boss currentBoss = new(Difficulty, "TestBoss");
                        GameManagerRef.SimulateBossCombat(currentBoss);
                        if (i != PathSteps.Count - 1)
                        {
                            ShowOptionsAfterInteractiveEvent();
                        }
                        break;
                }
                Thread.Sleep(random.Next(500, 1500));
            }
            PathCompleted();
        }

        /// <summary>
        /// Shuffles everything but the last element
        /// </summary>
        static List<PathStepType> ShufflePath(List<PathStepType> list)
        {
            Random random = new();
            List<PathStepType> itemsCopy = new(list);

            int copyCount = itemsCopy.Count - 1;

            for (int i = copyCount - 1; i > 0; i--)
            {
                int j = random.Next(0, copyCount--);
                (itemsCopy[j], itemsCopy[i]) = (itemsCopy[i], itemsCopy[j]);
            }

            return itemsCopy;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowOptionsAfterInteractiveEvent()
        {
            GameManager.HandleInputBuffering();
            Console.Write("Do you want to go back to (t)own, change your (e)quipment or (c)ontinue your adventure?: ");
            ConsoleKeyInfo key = Console.ReadKey();
            bool validInput = false;
            if (key.Key == ConsoleKey.T || key.Key == ConsoleKey.C || key.Key == ConsoleKey.E) validInput = true;
            while (!validInput)
            {
                Console.Write("\nNo choice was made, please try again: ");
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.T || key.Key == ConsoleKey.C || key.Key == ConsoleKey.E) validInput = true;
            }
            TextHelper.LineSpacing(0);
            if (key.Key == ConsoleKey.T)
            {
                TeleportToTown();
            }
            else if (key.Key == ConsoleKey.E)
            {
                PlayerRef.ChangeEquipment();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PathCompleted()
        {
            IsCompleted = true;
            float totalXpGained = XpOnCompletion + XpFromMobsOnPath;
            TextHelper.PrintTextInColor($"{PathCompletionMessage}, ", ConsoleColor.White, false);
            TextHelper.PrintTextInColor($"{totalXpGained} XP gained.", ConsoleColor.Blue, false);
            TextHelper.LineSpacing(0);
            PlayerRef.IncreaseXP(totalXpGained);
            TextHelper.LineSpacing(0);
            if (Difficulty == PathDifficulty.Final)
            {
                TextHelper.LineSpacing(0);
                TextHelper.PrintTextFile(Globals.OutroPath, true);
                TextHelper.PrintTextFile(Globals.CreditsPath, true);
                TextHelper.LineSpacing();

                TextHelper.PrintTextInColor("SHOULD DISPLAY NG+ OPTION", ConsoleColor.Magenta);
                Console.Write($"\nDo you want to (s)tart Journey {Globals.NewGameModifier + 1} or (q)uit : ");
                ConsoleKeyInfo key = Console.ReadKey();
                bool validInput = false;
                if (key.Key == ConsoleKey.S || key.Key == ConsoleKey.Q) validInput = true;
                while (!validInput)
                {
                    Console.Write("\nNo choice was made, please try again: ");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.S || key.Key == ConsoleKey.P) validInput = true;
                }

                if (key.Key == ConsoleKey.S)
                {
                    GameManagerRef.StartNewJourney();
                    return;
                }

                Environment.Exit(0);
            }
            TeleportToTown();
        }

        /// <summary>
        /// 
        /// </summary>
        public void TeleportToTown(string enemyName = "")
        {
            if (!PlayerRef.IsDead)
            {
                Console.Write("Teleporting back to town... ");
                if (!IsCompleted)
                {
                    PlayerRef.IncreaseXP(XpFromMobsOnPath);
                    TextHelper.PrintTextInColor($"{XpFromMobsOnPath} XP gained.\n\n", ConsoleColor.Blue, false);
                }
                else
                {
                    Console.WriteLine("\n");
                }
            }
            else
            {
                Console.WriteLine($"You've died to {enemyName}, teleporting back to town...");
                float xpLost = XpFromMobsOnPath * 0.5f;
                PlayerRef.IncreaseXP(XpFromMobsOnPath - xpLost);
                TextHelper.PrintTextInColor($"You've lost {xpLost} XP in the temporal twist...\n", ConsoleColor.DarkRed);
            }

            GameManagerRef.ShowTownOptions();
            Console.WriteLine();
        }
    }
}
