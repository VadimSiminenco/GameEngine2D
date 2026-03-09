using GameEngine2D.Entities.Structures;

namespace GameEngine2D.Construction
{
    public class BridgeBuilder : IStructureBuilder
    {
        private Structure _structure = new();

        public void Reset()
        {
            _structure = new Structure
            {
                Name = "Bridge",
                X = 10,
                Y = 2
            };
        }

        public void BuildBase()
        {
            _structure.BasePart = "Stone Supports";
        }

        public void BuildMiddle()
        {
            _structure.MiddlePart = "Wooden Span";
        }

        public void BuildTop()
        {
            _structure.TopPart = "Rope Rails";
        }

        public Structure GetResult()
        {
            return _structure;
        }
    }
}