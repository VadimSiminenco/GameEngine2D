namespace GameEngine2D.Entities.Characters
{
    public class Player : Character
    {
        public int Lives { get; }

        public Player(string name) : base(name, 100, 2, 8)
        {
            Lives = 3;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Lives={Lives}";
        }
    }
}