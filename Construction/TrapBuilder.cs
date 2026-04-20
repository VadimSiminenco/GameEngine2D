using GameEngine2D.Entities.Structures;

namespace GameEngine2D.Construction
{
    public class TrapBuilder : IStructureBuilder
    {
        private Structure _structure = new();

        public void Reset()
        {
            _structure = new Structure
            {
                Name = "Trap",
                X = 10,
                Y = 4,
                MapSymbol = 'T',
                SpriteKey = "trap_sprite"
            };
        }

        public void BuildBase()
        {
            _structure.BasePart = "Hidden Mechanism";
        }

        public void BuildMiddle()
        {
            _structure.MiddlePart = "Pressure Plate";
        }

        public void BuildTop()
        {
            _structure.TopPart = "Spikes";
        }

        public Structure GetResult()
        {
            return _structure;
        }
    }
}