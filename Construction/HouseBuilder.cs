using GameEngine2D.Entities.Structures;

namespace GameEngine2D.Construction
{
    public class HouseBuilder : IStructureBuilder
    {
        private Structure _structure = new();

        public void Reset()
        {
            _structure = new Structure
            {
                Name = "House",
                X = 34,
                Y = 8
            };
        }

        public void BuildBase()
        {
            _structure.BasePart = "Stone Foundation";
        }

        public void BuildMiddle()
        {
            _structure.MiddlePart = "Wooden Walls";
        }

        public void BuildTop()
        {
            _structure.TopPart = "Tile Roof";
        }

        public Structure GetResult()
        {
            return _structure;
        }
    }
}