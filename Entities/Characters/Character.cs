namespace GameEngine2D.Entities.Characters
{
    public abstract class Character
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        protected Character(string name, int health, int x, int y)
        {
            Name = name;
            Health = health;
            X = x;
            Y = y;
        }

        public virtual string GetInfo()
        {
            return $"{GetType().Name}: {Name}, HP={Health}, Pos=({X},{Y})";
        }
    }
}