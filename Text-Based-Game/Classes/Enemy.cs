﻿namespace Text_Based_Game.Classes
{
    internal class Enemy
    {
        public string Name { get; set; }
        private float StatMultiplier { get; set; }
        private readonly int BaseHp = 25;
        public int Hp { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public float DodgeChance { get; set; }
        private readonly int BaseXP = 12;
        public float XpDropped { get; set; }
        public Weapon? WeaponToDrop { get; set; }
        private Random Random { get; set; }


        // CONSTRUCTORS
        public Enemy(PathDifficulty difficulty)
        {
            Random = new();
            Name = RandomMobName();
            StatMultiplier = GetStatMultiplier(difficulty);
            if (Random.NextDouble() < 0.25f)
            {
                WeaponToDrop = GenerateWeaponToDrop(difficulty);
            }

            Hp = (int)(BaseHp * StatMultiplier * Globals.NewGameModifier);
            XpDropped = BaseXP * StatMultiplier * Globals.NewGameModifier;

            MinDamage = (int)(2 * StatMultiplier * Globals.NewGameModifier);
            MaxDamage = (int)(6 * StatMultiplier * Globals.NewGameModifier);

            DodgeChance = 0.1f;
        }
        public Enemy(PathDifficulty difficulty, string name) : this(difficulty)
        {
            Name = name;
        }

        // METHODS


        /// <summary>
        /// Returns a weapon out of one of two rarities depending on the difficulty
        /// </summary>
        private Weapon GenerateWeaponToDrop(PathDifficulty difficulty)
        {
            double randomDouble = Random.NextDouble();

            switch (difficulty)
            {
                case PathDifficulty.Medium:
                    if (randomDouble <= 0.125f)
                    {
                        return new Weapon(Rarity.Rare);
                    }
                    else
                    {
                        return new Weapon(Rarity.Uncommon);
                    }
                case PathDifficulty.Hard:
                    if (randomDouble <= 0.125f)
                    {
                        return new Weapon(Rarity.Epic);
                    }
                    else
                    {
                        return new Weapon(Rarity.Rare);
                    }
                case PathDifficulty.Final:
                    if (randomDouble <= 0.125f)
                    {
                        return new Weapon(Rarity.Legendary);
                    }
                    else
                    {
                        return new Weapon(Rarity.Epic);
                    }
                default:
                    if (randomDouble <= 0.125f)
                    {
                        return new Weapon(Rarity.Uncommon);
                    }
                    else
                    {
                        return new Weapon(Rarity.Common);
                    }
            }
        }

        /// <summary>
        /// Returns the stat multiplier for the specified difficulty
        /// </summary>
        private float GetStatMultiplier(PathDifficulty difficulty)
        {
            switch (difficulty)
            {
                case PathDifficulty.Medium:
                    return 1.5f;
                case PathDifficulty.Hard:
                    return 3.0f;
                case PathDifficulty.Final:
                    return 5.0f;
                default:
                    return 1.0f;
            }
        }

        /// <summary>
        /// Returns the amount of damage an attack will do
        /// </summary>
        public int CalculateAttack()
        {
            return Random.Next(MinDamage, MaxDamage + 1);
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
            string[] allNames = File.ReadAllLines(Globals.MobNamesPath);
            return allNames[Random.Next(allNames.Length)];
        }
    }
}
