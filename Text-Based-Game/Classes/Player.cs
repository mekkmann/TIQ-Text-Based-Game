namespace Text_Based_Game.Classes
{
    enum Location
    {
        Town,
        Path
    }

    internal class Player
    {
        public GameManager GameManagerRef { get; private set; }
        public readonly string Name = "Alaric";
        public int CurrentLevel = 1;
        public float XpToLevelUp = 987;
        public float CurrentXP = 0;
        public int AvailableSkillpoints = 0;
        public int SkillPointsPerLevel = 1;
        public int Vitality { get; set; }
        public int Strength { get; set; }
        public float MaxHp { get; set; }
        public float CurrentHp { get; set; }
        public Location CurrentLocation { get; set; }
        public int Respawns { get; set; }
        public bool IsDead { get; set; }

        // CONSTRUCTORS
        public Player(GameManager gameManagerRef)
        {
            Vitality = 5;
            Strength = 5;
            MaxHp = CalculateMaxHP();
            CurrentHp = MaxHp;
            Respawns = 3;
            IsDead = false;
            CurrentLocation = Location.Town;
            GameManagerRef = gameManagerRef;
        }

        // METHODS

        public void SetCurrentHpToMax()
        {
            CurrentHp = MaxHp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
            if (AvailableSkillpoints > 0)
            {
                Console.Write("\nWould you like to increase (v)itality or (s)trength, or (r)eturn to your adventure?: ");
                ConsoleKeyInfo key = Console.ReadKey();
                bool validInput = false;
                if (key.Key == ConsoleKey.V || key.Key == ConsoleKey.S || key.Key == ConsoleKey.R) validInput = true;
                while (!validInput)
                {
                    Console.Write("\nNo choice was made, please try again: ");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.V || key.Key == ConsoleKey.S || key.Key == ConsoleKey.R) validInput = true;
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
                    break;
                case ConsoleKey.V:
                    Vitality++;
                    MaxHp = CalculateMaxHP();
                    CurrentHp = MaxHp;
                    break;
            }

            AvailableSkillpoints--;
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
        public void LevelUp()
        {
            CurrentLevel++;
            CurrentXP -= XpToLevelUp;
            XpToLevelUp *= 1.11f;
            AvailableSkillpoints += SkillPointsPerLevel;
            Console.Write("Congratulations, ");
            TextHelper.PrintTextInColor($"you've reached lvl {CurrentLevel}", ConsoleColor.Blue, false);
            Console.Write($"! {SkillPointsPerLevel} new skill {(SkillPointsPerLevel == 1 ? "point" : "points")} available.");
            Console.WriteLine();
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
            TextHelper.PrintStringCharByChar("Hon hon hon");
            Console.WriteLine();
        }
    }
}
