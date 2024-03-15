namespace Text_Based_Game.Classes
{
    internal class Enemy
    {
        public string Name { get; set; }
        public float StatMultiplier { get; set; }
        public float HP { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public float DodgeChance { get; set; }
        public float XpDropped { get; set; }


        // CONSTRUCTORS
        public Enemy(string name)
        {
            Name = name;
        }
    }
}
