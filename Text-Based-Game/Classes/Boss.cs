namespace Text_Based_Game.Classes
{
    internal class Boss : Enemy
    {
        const string BossNamePath = "Content/bossNames.txt";
        public int BossMultiplier = 2;

        // CONSTRUCTORS
        public Boss(PathDifficulty difficulty) : base(difficulty)
        {
            Name = RandomBossName();
            HP *= BossMultiplier;
            XpDropped *= BossMultiplier;
            MinDamage *= BossMultiplier;
            MaxDamage *= 1000;
        }

        // METHODS
        private string RandomBossName()
        {
            Random random = new();
            string[] allNames = File.ReadAllLines(BossNamePath);
            return allNames[random.Next(allNames.Length - 1)];
        }
    }
}
