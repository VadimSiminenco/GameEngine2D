namespace GameEngine2D.Entities.Characters
{
    public class Npc : Character
    {
        public string Dialogue { get; }

        public Npc(string name) : base(name, 50, 6, 8)
        {
            Dialogue = "Welcome to the platformer world!";
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Dialogue=\"{Dialogue}\"";
        }
    }
}