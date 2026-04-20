namespace GameEngine2D.Entities.Characters
{
    public class Npc : Character
    {
        public string Dialogue { get; }

        public Npc(string name) : base(name, 50, 6, 8)
        {
            Dialogue = "Welcome to the platformer world!";
            MapSymbol = 'N';
            SpriteKey = "npc_sprite";
        }

        protected override void UpdateMainBehavior()
        {
        }

        protected override void UpdateSecondaryBehavior()
        {
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Dialogue=\"{Dialogue}\"";
        }
    }
}