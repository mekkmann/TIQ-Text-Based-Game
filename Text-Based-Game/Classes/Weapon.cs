namespace Text_Based_Game.Classes
{
    internal class Weapon
    {
        public string Name { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int MaxAttacksPerTurn { get; set; }
        public int MinAttacksPerTurn { get; set; }
        public int VitalityBonus { get; set; }
        public int StrengthBonus { get; set; }

        // CONSTRUCTORS

        public Weapon(string name, int minDamage, int maxDamage, int minAttacks, int maxAttacks, int vitalityBonus, int strengthBonus)
        {
            Name = name;
            MinAttacksPerTurn = minAttacks;
            MaxAttacksPerTurn = maxAttacks;
            VitalityBonus = vitalityBonus;
            StrengthBonus = strengthBonus;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }
    }
}
