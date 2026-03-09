namespace GameEngine2D.Entities.Characters
{
    public class Enemy : Character
    {
        public int Damage { get; set; }

        public Enemy(string name, int health, int damage, int x, int y)
            : base(name, health, x, y)
        {
            Damage = damage;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Damage={Damage}";
        }
    }
}