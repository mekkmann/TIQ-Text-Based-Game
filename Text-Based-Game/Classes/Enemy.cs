namespace Text_Based_Game.Classes
{
    internal class Enemy
    {
        const string mobNamesPath = "Content/mobNames.txt";

        public string Name { get; set; }
        public float StatMultiplier { get; set; }
        public int BaseHp = 25;
        public float HP { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public float DodgeChance { get; set; }
        public int BaseXP = 12;
        public float XpDropped { get; set; }


        // CONSTRUCTORS
        public Enemy(PathDifficulty difficulty)
        {
            Name = RandomMobName();
            switch (difficulty)
            {
                case PathDifficulty.Easy:
                    StatMultiplier = 1.0f;
                    break;
                case PathDifficulty.Medium:
                    StatMultiplier = 1.5f;
                    break;
                case PathDifficulty.Hard:
                    StatMultiplier = 3.0f;
                    break;
                case PathDifficulty.Final:
                    StatMultiplier = 5.0f;
                    break;
            }
            HP = BaseHp * StatMultiplier;
            XpDropped = BaseXP * StatMultiplier;

            MinDamage = (int)(2 * StatMultiplier);
            MaxDamage = (int)(6 * StatMultiplier);

            DodgeChance = 0.125f;

        }

        // METHODS
        private string RandomMobName()
        {
            Random random = new();
            string[] allNames = File.ReadAllLines(mobNamesPath);
            return allNames[random.Next(allNames.Length)];
        }
    }
}
