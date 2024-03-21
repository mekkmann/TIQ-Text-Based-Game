namespace Text_Based_Game.Classes
{
    internal class Enemy
    {
        const string MobNamesPath = "Content/mobNames.txt";
        public string Name { get; set; }
        public float StatMultiplier { get; set; }
        public int BaseHp = 25;
        public int Hp { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public float DodgeChance { get; set; }
        public int BaseXP = 12;
        public float XpDropped { get; set; }
        public object? ItemToDrop { get; set; }


        // CONSTRUCTORS
        public Enemy(PathDifficulty difficulty)
        {
            Random random = new();
            Name = RandomMobName();
            double randomDouble = random.NextDouble();
            switch (difficulty)
            {
                case PathDifficulty.Easy:
                    StatMultiplier = 1.0f;
                    if (randomDouble <= 0.125f)
                    {
                        ItemToDrop = new Weapon(Rarity.Uncommon);
                        break;
                    }
                    else if (randomDouble <= 0.25f)
                    {
                        ItemToDrop = new Weapon(Rarity.Common);
                        break;
                    }
                    break;
                case PathDifficulty.Medium:
                    StatMultiplier = 1.5f;
                    if (randomDouble <= 0.125f)
                    {
                        ItemToDrop = new Weapon(Rarity.Rare);
                        break;
                    }
                    else if (randomDouble <= 0.25f)
                    {
                        ItemToDrop = new Weapon(Rarity.Uncommon);
                        break;
                    }
                    break;
                case PathDifficulty.Hard:
                    StatMultiplier = 3.0f;
                    if (randomDouble <= 0.125f)
                    {
                        ItemToDrop = new Weapon(Rarity.Epic);
                        break;
                    }
                    else if (randomDouble <= 0.25f)
                    {
                        ItemToDrop = new Weapon(Rarity.Rare);
                        break;
                    }
                    break;
                case PathDifficulty.Final:
                    StatMultiplier = 5.0f;
                    if (randomDouble <= 0.125f)
                    {
                        ItemToDrop = new Weapon(Rarity.Legendary);
                        break;
                    }
                    else if (randomDouble <= 0.25f)
                    {
                        ItemToDrop = new Weapon(Rarity.Epic);
                        break;
                    }
                    break;
            }
            Hp = (int)(BaseHp * StatMultiplier);
            XpDropped = BaseXP * StatMultiplier;

            MinDamage = (int)(2 * StatMultiplier);
            MaxDamage = (int)(6 * StatMultiplier);

            DodgeChance = 0.1f;
        }

        public Enemy(PathDifficulty difficulty, string name)
        {
            Random random = new();
            double randomDouble = random.NextDouble();
            Name = name;
            switch (difficulty)
            {
                case PathDifficulty.Easy:
                    StatMultiplier = 1.0f;
                    if (randomDouble <= 0.125f)
                    {
                        ItemToDrop = new Weapon(Rarity.Uncommon);
                        break;
                    }
                    else if (randomDouble <= 0.25f)
                    {
                        ItemToDrop = new Weapon(Rarity.Common);
                        break;
                    }
                    break;
                case PathDifficulty.Medium:
                    StatMultiplier = 1.5f;
                    if (randomDouble <= 0.125f)
                    {
                        ItemToDrop = new Weapon(Rarity.Rare);
                        break;
                    }
                    else if (randomDouble <= 0.25f)
                    {
                        ItemToDrop = new Weapon(Rarity.Uncommon);
                        break;
                    }
                    break;
                case PathDifficulty.Hard:
                    StatMultiplier = 3.0f;
                    if (randomDouble <= 0.125f)
                    {
                        ItemToDrop = new Weapon(Rarity.Epic);
                        break;
                    }
                    else if (randomDouble <= 0.25f)
                    {
                        ItemToDrop = new Weapon(Rarity.Rare);
                        break;
                    }
                    break;
                case PathDifficulty.Final:
                    StatMultiplier = 5.0f;
                    if (randomDouble <= 0.125f)
                    {
                        ItemToDrop = new Weapon(Rarity.Legendary);
                        break;
                    }
                    else if (randomDouble <= 0.25f)
                    {
                        ItemToDrop = new Weapon(Rarity.Epic);
                        break;
                    }
                    break;
            }
            Hp = (int)(BaseHp * StatMultiplier);
            XpDropped = BaseXP * StatMultiplier;

            MinDamage = (int)(2 * StatMultiplier);
            MaxDamage = (int)(6 * StatMultiplier);

            DodgeChance = 0.1f;
        }

        // METHODS

        /// <summary>
        /// 
        /// </summary>
        public int CalculateAttack()
        {
            Random random = new();

            return random.Next(MinDamage, MaxDamage + 1);
        }

        /// <summary>
        /// Subtracts damage from Hp
        /// </summary>
        public void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp < 0)
            {
                Hp = 0;
            }
        }

        /// <summary>
        /// Reads file containing mob names and returns a random name
        /// </summary>
        private string RandomMobName()
        {
            Random random = new();
            string[] allNames = File.ReadAllLines(MobNamesPath);
            return allNames[random.Next(allNames.Length)];
        }
    }
}
