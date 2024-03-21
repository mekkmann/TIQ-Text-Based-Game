﻿namespace Text_Based_Game.Classes
{
    internal class Boss : Enemy
    {
        const string BossNamePath = "Content/bossNames.txt";
        private readonly float BossMultiplier = 1.5f;

        // CONSTRUCTORS
        public Boss(PathDifficulty difficulty) : base(difficulty)
        {
            Name = RandomBossName();
            Hp = (int)(Hp * BossMultiplier);
            XpDropped *= BossMultiplier;
            MinDamage = (int)(MinDamage * BossMultiplier);
            MaxDamage = (int)(MaxDamage * BossMultiplier);
        }
        public Boss(PathDifficulty difficulty, string name) : base(difficulty, name)
        {
            Name = name;
            Hp = (int)(Hp * BossMultiplier);
            XpDropped *= BossMultiplier;
            MinDamage = (int)(MinDamage * BossMultiplier);
            MaxDamage = (int)(MaxDamage * BossMultiplier);
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
