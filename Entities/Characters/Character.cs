namespace GameEngine2D.Entities.Characters
{
    public abstract class Character
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public char MapSymbol { get; protected set; } = '?';
        public string SpriteKey { get; protected set; } = "unknown_character_sprite";

        protected Character(string name, int health, int x, int y)
        {
            Name = name;
            Health = health;
            X = x;
            Y = y;
        }

        public virtual void Move(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public virtual void MoveLeft()
        {
            Move(-1, 0);
        }

        public virtual void MoveRight()
        {
            Move(1, 0);
        }

        public void Update()
        {
            if (Health <= 0)
            {
                return;
            }

            UpdateMainBehavior();
            UpdateSecondaryBehavior();
        }

        protected abstract void UpdateMainBehavior();
        protected abstract void UpdateSecondaryBehavior();

        public virtual string GetInfo()
        {
            return $"{GetType().Name}: {Name}, HP={Health}, Pos=({X},{Y}), Symbol={MapSymbol}, Sprite={SpriteKey}";
        }
    }
}