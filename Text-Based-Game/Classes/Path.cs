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
            GameManagerRef = gameManager;
            Random random = new();

            PathStartMessage = pathStartMessage;
            PathCompletionMessage = pathCompletionMessage;
            PathLength = pathLength;
            PathSteps = [];
            for (int i = 0; i < pathLength; i++)
            {
                int randomIndex = random.Next(Enum.GetValues(typeof(PathStepType)).Length - 1);

                if (i == pathLength - 1)
                {
                    PathSteps.Add(new(PathStepType.BossFight));
                    continue;
                }

                switch (randomIndex)
                {
                    case (int)PathStepType.Walking:
                        PathSteps.Add(new(PathStepType.Walking));
                        break;
                    case (int)PathStepType.PlayerTalk:
                        PathSteps.Add(new(PathStepType.PlayerTalk));
                        break;
                    case (int)PathStepType.MobFight:
                        PathSteps.Add(new(PathStepType.MobFight));
                        break;
                }
            }

            IsCompleted = false;
            PlayerRef = player;
            Difficulty = difficulty;
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
                    XpOnCompletion = 400f;
                    break;
            }
            XpFromMobsOnPath = 0;
        }

        // METHODS

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            PlayerRef.CurrentLocation = Location.Path;

            Console.WriteLine(PathStartMessage);
            Random random = new();
            Thread.Sleep(random.Next(500, 1500));
            foreach (PathStep step in PathSteps)
            {
                switch (step.Type)
                {
                    case PathStepType.Walking:
                        Console.WriteLine("*walking*");
                        break;
                    case PathStepType.PlayerTalk:
                        PlayerRef.SpeakAboutEnvironment();
                        break;
                    case PathStepType.MobFight:
                        TextHelper.PrintTextInColor("*should be a mob fight*", ConsoleColor.DarkYellow, true);
                        break;
                    case PathStepType.BossFight:
                        TextHelper.PrintTextInColor("*should be a boss fight*", ConsoleColor.DarkRed, true);
                        break;
                }
                Thread.Sleep(random.Next(500, 1500));
            }
            PathCompleted();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PathCompleted()
        {
            float totalXpGained = XpOnCompletion + XpFromMobsOnPath;
            Console.Write($"{PathCompletionMessage}, ");
            TextHelper.PrintTextInColor($"{totalXpGained} XP gained.", ConsoleColor.Blue, false);
            Console.WriteLine();
            PlayerRef.IncreaseXP(totalXpGained);
            TeleportToTown();
        }

        /// <summary>
        /// 
        /// </summary>
        public void TeleportToTown()
        {
            if (!PlayerRef.IsDead)
            {
                Console.WriteLine("Teleporting back to town...\n");
            }
            else
            {
                Console.WriteLine("You've died to [enemyName], teleporting back to town...");
                Console.WriteLine("You've lost [X] XP and [lootName] in the temporal twist...\n");

            }

            GameManagerRef.ShowTownOptions();
            Console.WriteLine();
        }
    }
}
