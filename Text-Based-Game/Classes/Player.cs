namespace Text_Based_Game.Classes
{
    enum Location
    {
        Town,
        Path
    }

    enum Stat
    {
        Vitality,
        Strength
    }

    internal class Player
    {
        const string EnvironmentObservationsPath = "Content/environmentObservations.txt";
        const int StartingVitality = 100;
        const int StartingStrength = 5;
        public GameManager GameManagerRef { get; private set; }
        public string[] EnvironmentObservations { get; private set; }
        public Weapon EquippedWeapon = new(Rarity.Common, "Fists");
        public List<Weapon> WeaponsInBag = [];
        public readonly string Name = "Alaric";
        public int CurrentLevel = 1;
        public float XpToLevelUp = 200;
        public float CurrentXP = 0;
        public int TotalSkillPointsGained = 0;
        public int AvailableSkillpoints = 0;
        public int SkillPointsPerLevel = 1;
        public int Vitality { get; set; }
        public int VitalitySkillPoints { get; set; }
        public int Strength { get; set; }
        public int StrengthSkillPoints { get; set; }
        public float MaxHp { get; set; }
        public float CurrentHp { get; set; }
        public Location CurrentLocation { get; set; }
        public int Respawns { get; set; }
        public bool IsDead { get; set; }

        // CONSTRUCTORS
        public Player(GameManager gameManagerRef)
        {
            Vitality = StartingVitality;
            Strength = StartingStrength;
            MaxHp = CalculateMaxHP();
            CurrentHp = MaxHp;
            Respawns = 3;
            IsDead = false;
            CurrentLocation = Location.Town;
            GameManagerRef = gameManagerRef;
            WeaponsInBag.Add(new(Rarity.Common));
            WeaponsInBag.Add(new(Rarity.Uncommon));
            WeaponsInBag.Add(new(Rarity.Legendary));
            EnvironmentObservations = File.ReadAllLines(EnvironmentObservationsPath);
        }

        // METHODS

        public void PickUpLoot(object loot)
        {
            if (loot.GetType() == typeof(Weapon))
            {
                WeaponsInBag.Add((Weapon)loot);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] CalculateAttack()
        {
            Random random = new();

            int numberOfAttacks = random.Next(EquippedWeapon.MinAttacksPerTurn, EquippedWeapon.MaxAttacksPerTurn + 1);

            int totalDamage = 0;
            for (int i = 0; i < numberOfAttacks; i++)
            {
                totalDamage += random.Next(EquippedWeapon.MinDamage + Strength, EquippedWeapon.MaxDamage + Strength + 1);
            }


            return [numberOfAttacks, totalDamage];
        }

        /// <summary>
        /// 
        /// </summary>
        public void ChangeEquipment()
        {
            Console.WriteLine("Weapons:");
            for (var i = 0; i < WeaponsInBag.Count; i++)
            {
                Weapon temp = WeaponsInBag[i];
                Console.WriteLine($"{i + 1}. {temp.Name} ({temp.Rarity})");
                Console.WriteLine($"    Damage: {temp.MinDamage} - {temp.MaxDamage}");
                if (temp.MinAttacksPerTurn == temp.MaxAttacksPerTurn)
                {
                    Console.WriteLine($"    Attacks per turn: {temp.MinAttacksPerTurn}");
                }
                else
                {
                    Console.WriteLine($"    Attacks per turn: {temp.MinAttacksPerTurn} - {temp.MaxAttacksPerTurn}");
                }
                Console.WriteLine($"    Vitality Boost: {temp.VitalityBonus}");
                Console.WriteLine($"    Strength Boost: {temp.StrengthBonus}");
            }
            Console.Write("Equip weapon by pressing the corresponding number or (r)eturn to previous selection (Please press enter as well): ");

            bool validInput = false;
            do
            {
                string input = Console.ReadLine();
                bool inputAsInt = Int32.TryParse(input, out int valueAsInt);
                if (input.ToLower() == "r" || valueAsInt == 1 || valueAsInt == 2 || valueAsInt == 3)
                {
                    validInput = true;
                    if (input == "r")
                    {
                        GameManagerRef.CurrentPath?.ShowOptionsAfterInteractiveEvent();
                        return;
                    }
                    if (valueAsInt == 1)
                    {
                        EquipWeapon(WeaponsInBag[0]);
                    }
                    else if (valueAsInt == 2)
                    {
                        EquipWeapon(WeaponsInBag[1]);
                    }
                    else if (valueAsInt == 3)
                    {
                        EquipWeapon(WeaponsInBag[2]);
                    }
                }
            } while (!validInput);
            GameManagerRef.CurrentPath?.ShowOptionsAfterInteractiveEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        public void EquipWeapon(Weapon weapon)
        {
            DecreaseStat(Stat.Vitality, EquippedWeapon.VitalityBonus);
            DecreaseStat(Stat.Strength, EquippedWeapon.StrengthBonus);
            EquippedWeapon = weapon;
            IncreaseStat(Stat.Vitality, weapon.VitalityBonus);
            IncreaseStat(Stat.Strength, weapon.StrengthBonus);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetCurrentHpToMax()
        {
            CurrentHp = MaxHp;
        }

        /// <summary>
        /// 
        /// </summary>
        private int CalculateMaxHP()
        {
            return 20 * Vitality;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowStats()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Your Stats: ");
            Console.WriteLine($"Level: {CurrentLevel}");
            Console.WriteLine($"Current XP: {(int)CurrentXP}");
            Console.WriteLine($"XP needed for next level: {(int)XpToLevelUp}");
            Console.WriteLine($"{AvailableSkillpoints} stat points available");
            Console.WriteLine($"Max HP: {MaxHp}");
            Console.WriteLine($"Vitality: {Vitality}");
            Console.WriteLine($"Strength: {Strength}");
            TextHelper.LineSpacing(0);
            Console.WriteLine($"Equipped Weapon: {EquippedWeapon.Name}");
            Console.WriteLine($"    Damage: {EquippedWeapon.MinDamage} - {EquippedWeapon.MaxDamage}");
            if (EquippedWeapon.MinAttacksPerTurn == EquippedWeapon.MaxAttacksPerTurn)
            {
                Console.WriteLine($"    Attacks per turn: {EquippedWeapon.MinAttacksPerTurn}");
            }
            else
            {
                Console.WriteLine($"    Attacks per turn: {EquippedWeapon.MinAttacksPerTurn} - {EquippedWeapon.MaxAttacksPerTurn}");
            }
            Console.WriteLine($"    Vitality Boost: {EquippedWeapon.VitalityBonus}");
            Console.WriteLine($"    Strength Boost: {EquippedWeapon.StrengthBonus}");
            if (AvailableSkillpoints > 0)
            {
                Console.Write("\nWould you like to increase (v)itality or (s)trength, r(e)spec or (r)eturn to your adventure?: ");
                ConsoleKeyInfo key = Console.ReadKey();
                bool validInput = false;
                if (key.Key == ConsoleKey.V || key.Key == ConsoleKey.S || key.Key == ConsoleKey.R || key.Key == ConsoleKey.E) validInput = true;
                while (!validInput)
                {
                    Console.Write("\nNo choice was made, please try again: ");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.V || key.Key == ConsoleKey.S || key.Key == ConsoleKey.R || key.Key == ConsoleKey.E) validInput = true;
                }
                Console.WriteLine("\n");
                switch (key.Key)
                {
                    case ConsoleKey.R:
                        GameManagerRef.ShowTownOptions();
                        break;
                    case ConsoleKey.V:
                    case ConsoleKey.S:
                        IncreaseStat(key);
                        ShowStats();
                        break;
                    case ConsoleKey.E:
                        ResetSkillPoints();
                        ShowStats();
                        break;
                }
            }
            else
            {
                Console.Write("\nPress any key to return to your adventure: ");
                _ = Console.ReadKey();
                Console.WriteLine();
                GameManagerRef.ShowTownOptions();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void IncreaseStat(ConsoleKeyInfo key)
        {
            if (AvailableSkillpoints <= 0) return;

            switch (key.Key)
            {
                case ConsoleKey.S:
                    Strength++;
                    StrengthSkillPoints++;
                    break;
                case ConsoleKey.V:
                    Vitality++;
                    VitalitySkillPoints++;
                    MaxHp = CalculateMaxHP();
                    CurrentHp = MaxHp;
                    break;
            }

            AvailableSkillpoints--;
        }

        /// <summary>
        /// 
        /// </summary>
        public void IncreaseStat(Stat stat, int amount)
        {
            switch (stat)
            {
                case Stat.Vitality:
                    Vitality += amount;
                    MaxHp = CalculateMaxHP();
                    break;
                case Stat.Strength:
                    Strength += amount;
                    break;
            }
        }

        public void DecreaseStat(Stat stat, int amount)
        {
            switch (stat)
            {
                case Stat.Vitality:
                    Vitality -= amount;
                    MaxHp = CalculateMaxHP();
                    if (CurrentHp > MaxHp)
                    {
                        SetCurrentHpToMax();
                    }
                    break;
                case Stat.Strength:
                    Strength -= amount;
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void IncreaseXP(float xpGained)
        {
            CurrentXP += xpGained;
            if (CurrentXP >= XpToLevelUp)
            {
                LevelUp();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DecreaseXP(float xpLost)
        {
            CurrentXP -= xpLost;
            if (CurrentXP < 0)
            {
                CurrentXP = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetSkillPoints()
        {
            Vitality -= VitalitySkillPoints;
            Strength -= StrengthSkillPoints;

            AvailableSkillpoints += VitalitySkillPoints + StrengthSkillPoints;

            VitalitySkillPoints = 0;
            StrengthSkillPoints = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void LevelUp()
        {
            CurrentLevel++;

            // TESTING
            if (CurrentLevel == 5)
            {
                GameManagerRef.CanTakeFinalPath = true;
            }
            /////////////////////

            CurrentXP -= XpToLevelUp;
            XpToLevelUp *= 1.11f;
            AvailableSkillpoints += SkillPointsPerLevel;
            TotalSkillPointsGained += SkillPointsPerLevel;
            Console.Write("Congratulations, ");
            TextHelper.PrintTextInColor($"you've reached lvl {CurrentLevel}", ConsoleColor.Blue, false);
            Console.Write($"! {SkillPointsPerLevel} new skill {(SkillPointsPerLevel == 1 ? "point" : "points")} available.");
            Console.WriteLine();
            if (CurrentXP >= XpToLevelUp)
            {
                LevelUp();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void TakeDamage(int damageTaken)
        {
            CurrentHp -= damageTaken;
            if (CurrentHp <= 0)
            {
                CurrentHp = 0;
                IsDead = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Heal(int amountHealed)
        {
            // if player is already at max hp
            if (CurrentHp == MaxHp)
            {
                return;
            }

            // heal player for amountHealed
            CurrentHp += amountHealed;
            // if hp goes over maxHp
            if (CurrentHp >= MaxHp)
            {
                // set current hp to max hp
                CurrentHp = MaxHp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SpeakAboutEnvironment()
        {
            Random random = new();
            string randomSentence = EnvironmentObservations[random.Next(EnvironmentObservations.Length)];
            TextHelper.PrintStringCharByChar(randomSentence, ConsoleColor.White);
            Console.WriteLine();
        }
    }
}
