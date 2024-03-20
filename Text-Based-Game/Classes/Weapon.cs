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
            int minAttacks,
            int maxAttacks,
            int vitalityBonus,
            int strengthBonus,
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
            MinAttacksPerTurn = minAttacks;
            MaxAttacksPerTurn = maxAttacks;
            VitalityBonus = vitalityBonus;
            StrengthBonus = strengthBonus;
            MinDamage = GenerateMinDamage();
            MaxDamage = 100;
        }


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
        /// <returns></returns>
        private string WeaponName()
        {
            Random random = new();

            string[] allNames = File.ReadAllLines(WeaponNamePath);
            return allNames[random.Next(allNames.Length)];
        }
    }
}
