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
            Name = name;
            PathLength = pathLength;

            PathSteps = [
                new(PathStepType.Walking), new(PathStepType.Walking),
                new(PathStepType.PlayerTalk), new(PathStepType.Walking),
                new(PathStepType.Walking), new(PathStepType.Walking),
                ];

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

        public void Start()
        {
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

        private void PathCompleted()
        {
            IsCompleted = true;
            Console.WriteLine($"{Name} completed, {XpOnCompletion + XpFromMobsOnPath} XP gained.");
        }
    }
}
