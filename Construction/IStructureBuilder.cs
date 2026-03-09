using GameEngine2D.Entities.Structures;

namespace GameEngine2D.Construction
{
    public interface IStructureBuilder
    {
        void Reset();
        void BuildBase();
        void BuildMiddle();
        void BuildTop();
        Structure GetResult();
    }
}