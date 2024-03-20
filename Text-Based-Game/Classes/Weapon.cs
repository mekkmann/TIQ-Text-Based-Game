namespace Text_Based_Game.Classes
{
    enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
    internal class Weapon
    {
        const string WeaponNamePath = "Content/weaponNames.txt";
        public string Name { get; set; }
        public Rarity Rarity { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int MaxAttacksPerTurn { get; set; }
        public int MinAttacksPerTurn { get; set; }
        public int VitalityBonus { get; set; }
        public int StrengthBonus { get; set; }

        // CONSTRUCTORS

        public Weapon(
            Rarity rarity,
            string name = "Placeholder"
            )
        {
            if (name == "Placeholder")
            {
                Name = WeaponName();
            }
            else
            {
                Name = name;
            }
            Rarity = rarity;
            MinAttacksPerTurn = GenerateMinAttacks();
            MaxAttacksPerTurn = GenerateMaxAttacks();
            VitalityBonus = GenerateStatBonus();
            StrengthBonus = GenerateStatBonus();
            MinDamage = GenerateMinDamage();
            MaxDamage = GenerateMaxDamage();
        }

        /// <summary>
        /// 
        /// </summary>
        private int GenerateStatBonus()
        {
            Random random = new();
            switch (Rarity)
            {
                case Rarity.Common:
                    return 0;
                case Rarity.Uncommon:
                    if (random.NextDouble() < 0.1f)
                    {
                        return random.Next(3, 4 + 1);
                    }
                    else
                    {
                        return random.Next(0, 2 + 1);
                    }
                case Rarity.Rare:
                    if (random.NextDouble() < 0.1f)
                    {
                        return random.Next(3, 6 + 1);
                    }
                    else
                    {
                        return random.Next(1, 5 + 1);
                    }
                case Rarity.Epic:
                    if (random.NextDouble() < 0.1f)
                    {
                        return random.Next(5, 14 + 1);
                    }
                    else
                    {
                        return random.Next(3, 12 + 1);
                    }
                case Rarity.Legendary:
                    if (random.NextDouble() < 0.1f)
                    {
                        return 20;
                    }
                    else
                    {
                        return random.Next(5, 15 + 1);
                    }
                default:
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private int GenerateMaxAttacks()
        {
            Random random = new();
            if (random.NextDouble() < 0.2f)
            {
                return MinAttacksPerTurn + 1;
            }
            else
            {
                return MinAttacksPerTurn;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int GenerateMinAttacks()
        {
            Random random = new();
            switch (Rarity)
            {
                case Rarity.Common:
                case Rarity.Uncommon:
                    if (random.NextDouble() < 0.1f)
                    {
                        return random.Next(1, 2 + 1);
                    }
                    else
                    {
                        return 1;
                    }
                case Rarity.Rare:
                    if (random.NextDouble() < 0.1f)
                    {
                        return random.Next(1, 3 + 1);
                    }
                    else
                    {
                        return random.Next(1, 2 + 1);
                    }
                case Rarity.Epic:
                    if (random.NextDouble() < 0.1f)
                    {
                        return random.Next(2, 3 + 1);
                    }
                    else
                    {
                        return random.Next(1, 3 + 1);
                    }
                case Rarity.Legendary:
                    if (random.NextDouble() < 0.1f)
                    {
                        return 4;
                    }
                    else
                    {
                        return random.Next(1, 3 + 1);
                    }
                default:
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private int GenerateMaxDamage()
        {
            Random random = new();
            switch (Rarity)
            {
                case Rarity.Common:
                    if (random.NextDouble() < 0.25f)
                    {
                        return 5;
                    }
                    else
                    {
                        return random.Next(2, 4 + 1);
                    }
                case Rarity.Uncommon:
                    if (random.NextDouble() < 0.25f)
                    {
                        return 10;
                    }
                    else
                    {
                        return random.Next(3, 8 + 1);
                    }
                case Rarity.Rare:
                    if (random.NextDouble() < 0.25f)
                    {
                        return 25;
                    }
                    else
                    {
                        return random.Next(10, 17 + 1);
                    }
                case Rarity.Epic:
                    if (random.NextDouble() < 0.25f)
                    {
                        return 50;
                    }
                    else
                    {
                        return random.Next(20, 35 + 1);
                    }
                case Rarity.Legendary:
                    if (random.NextDouble() < 0.25f)
                    {
                        return 75;
                    }
                    else
                    {
                        return random.Next(40, 70 + 1);
                    }
                default:
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private int GenerateMinDamage()
        {
            Random random = new();
            switch (Rarity)
            {
                case Rarity.Common:
                    if (random.NextDouble() < 0.25f)
                    {
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                case Rarity.Uncommon:
                    if (random.NextDouble() < 0.25f)
                    {
                        return 4;
                    }
                    else
                    {
                        return random.Next(1, 2 + 1);
                    }
                case Rarity.Rare:
                    if (random.NextDouble() < 0.25f)
                    {
                        return 6;
                    }
                    else
                    {
                        return random.Next(2, 4 + 1);
                    }
                case Rarity.Epic:
                    if (random.NextDouble() < 0.25f)
                    {
                        return random.Next(10, 15 + 1);

                    }
                    else
                    {
                        return random.Next(4, 8 + 1);
                    }
                case Rarity.Legendary:
                    if (random.NextDouble() < 0.25f)
                    {
                        return random.Next(25, 30 + 1);
                    }
                    else
                    {
                        return random.Next(15, 20 + 1);
                    }
                default:
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string WeaponName()
        {
            Random random = new();

            string[] allNames = File.ReadAllLines(WeaponNamePath);
            return allNames[random.Next(allNames.Length)];
        }
    }
}
