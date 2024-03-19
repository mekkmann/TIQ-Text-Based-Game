namespace Text_Based_Game.Classes
{
    enum PathDifficulty
    {
        Easy,
        Medium,
        Hard,
        Final
    }

    internal class GamePath
    {
        const string OutroPath = "Content/outro.txt";
        public GameManager GameManagerRef { get; private set; }
        public string PathStartMessage { get; set; }
        public string PathCompletionMessage { get; set; }
        public PathDifficulty Difficulty { get; set; }
        public int PathLength { get; set; }
        public List<PathStep> PathSteps { get; set; }
        public bool IsCompleted { get; set; }
        public Player PlayerRef { get; set; }
        public float XpOnCompletion { get; set; }
        public float XpFromMobsOnPath { get; set; }

        // CONSTRUCTORS
        public GamePath(string pathStartMessage, string pathCompletionMessage, int pathLength, Player player, PathDifficulty difficulty, GameManager gameManager)
        {
            Random random = new();
            GameManagerRef = gameManager;
            PathStartMessage = pathStartMessage;
            PathCompletionMessage = pathCompletionMessage;

            Difficulty = difficulty;
            PathLength = pathLength;
            PathSteps = MakePath(Difficulty);

            foreach (var step in PathSteps)
            {
                Console.WriteLine(step.Type);
            }
            IsCompleted = false;
            PlayerRef = player;
            switch (Difficulty)
            {
                case PathDifficulty.Easy:
                    XpOnCompletion = 1000f;
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

        private List<PathStep> MakePath(PathDifficulty difficulty)
        {
            Random random = new();
            List<PathStep> steps = [];

            switch (difficulty)
            {
                case PathDifficulty.Easy:
                    //for (int i = 0; i <= PathLength / 2; i++)
                    //{
                    //    steps.Add(new(PathStepType.MobFight));
                    //}
                    //for (int i = 0; i < PathLength / 2; i++)
                    //{
                    //    steps.Add(new(PathStepType.Walking));
                    //}
                    //for (int i = 0; i < PathLength / 2; i++)
                    //{
                    //    steps.Add(new(PathStepType.PlayerTalk));
                    //}
                    steps.Add(new(PathStepType.Walking));
                    steps.Add(new(PathStepType.PlayerTalk));
                    steps.Add(new(PathStepType.MobFight));
                    steps.Add(new(PathStepType.BossFight));
                    break;
                case PathDifficulty.Medium:
                    for (int i = 0; i <= PathLength / 2; i++)
                    {
                        steps.Add(new(PathStepType.MobFight));
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(new(PathStepType.Walking));
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(new(PathStepType.PlayerTalk));
                    }
                    steps.Add(new(PathStepType.BossFight));
                    steps.Add(new(PathStepType.BossFight));
                    break;
                case PathDifficulty.Hard:
                    for (int i = 0; i <= PathLength / 2; i++)
                    {
                        steps.Add(new(PathStepType.MobFight));
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(new(PathStepType.Walking));
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(new(PathStepType.PlayerTalk));
                    }
                    steps.Add(new(PathStepType.BossFight));
                    steps.Add(new(PathStepType.BossFight));
                    steps.Add(new(PathStepType.BossFight));
                    break;
                case PathDifficulty.Final:
                    for (int i = 0; i <= PathLength / 2; i++)
                    {
                        steps.Add(new(PathStepType.BossFight));
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(new(PathStepType.Walking));
                    }
                    for (int i = 0; i < PathLength / 2; i++)
                    {
                        steps.Add(new(PathStepType.PlayerTalk));
                    }
                    steps.Add(new(PathStepType.BossFight));
                    break;
            }
            Console.WriteLine(PathLength);
            Console.WriteLine(steps.Count);

            return ShufflePath(steps);
        }


        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            PlayerRef.CurrentLocation = Location.Path;

            TextHelper.PrintStringCharByChar(PathStartMessage, ConsoleColor.White);
            TextHelper.LineSpacing(0);
            Random random = new();
            Thread.Sleep(random.Next(500, 1500));
            for (int i = 0; i < PathSteps.Count; i++)
            {
                switch (PathSteps[i].Type)
                {
                    case PathStepType.Walking:
                        TextHelper.PrintTextInColor("*walking*", ConsoleColor.DarkGray);
                        break;
                    case PathStepType.PlayerTalk:
                        PlayerRef.SpeakAboutEnvironment();
                        break;
                    case PathStepType.MobFight:
                        // GENERATE MOB AND SIMULATE FIGHT
                        Enemy currentEnemy = new(Difficulty);
                        GameManagerRef.SimulateRegularCombat(currentEnemy);
                        ShowOptionsAfterInteractiveEvent();
                        break;
                    case PathStepType.BossFight:
                        //TextHelper.PrintTextInColor("*should be a boss fight*", ConsoleColor.DarkRed, true);
                        Boss currentBoss = new(Difficulty);
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
        static List<PathStep> ShufflePath(List<PathStep> items)
        {
            Random random = new();
            List<PathStep> itemsCopy = new(items);

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
                TextHelper.PrintTextFile(OutroPath, true);
                TextHelper.LineSpacing();
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
