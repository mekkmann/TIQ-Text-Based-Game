namespace Text_Based_Game.Classes
{
    internal class Player
    {
        public readonly string Name = "Alaric";
        public int Vitality { get; set; }
        public int Strength { get; set; }
        public float MaxHp { get; set; }
        public float CurrentHp { get; set; }
        public int Respawns { get; set; }
        public bool IsDead { get; set; }

        // CONSTRUCTORS
        public Player()
        {
            Vitality = 10;
            Strength = 5;
            MaxHp = 100;
            CurrentHp = MaxHp;
            Respawns = 3;
            IsDead = false;
        }

        // METHODS

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
