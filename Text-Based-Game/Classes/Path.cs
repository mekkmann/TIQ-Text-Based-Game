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
        public string Name { get; set; }
        public PathDifficulty Difficulty { get; set; }
        public int PathLength { get; set; }
        public List<PathStep> PathSteps { get; set; }
        public bool IsCompleted { get; set; }
        public Player PlayerRef { get; set; }
        public float XpOnCompletion { get; set; }
        public float XpFromMobsOnPath { get; set; }

        // CONSTRUCTORS
        public GamePath(string name, int pathLength, Player player, PathDifficulty difficulty)
        {
            Random random = new();

            Name = name;
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
                    XpOnCompletion = 100f;
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

            Console.WriteLine($"This time, I shall venture down {Name}");
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
                        Console.WriteLine("*should be a mob fight*");
                        break;
                    case PathStepType.BossFight:
                        Console.WriteLine("*should be a boss fight*");
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
            Console.WriteLine($"{Name} completed, {XpOnCompletion + XpFromMobsOnPath} XP gained.");
            TeleportToTown();
        }

        /// <summary>
        /// 
        /// </summary>
        public void TeleportToTown()
        {
            if (!PlayerRef.IsDead)
            {
                Console.WriteLine("Teleporting back to town...");
            }
            else
            {
                Console.WriteLine("You've died to [enemyName], teleporting back to town...");
                Console.WriteLine("You've lost [X] XP and [lootName] in the temporal twist...");
            }

            PlayerRef.CurrentLocation = Location.Town;
        }
    }
}
