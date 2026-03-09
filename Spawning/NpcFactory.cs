using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Spawning
{
    public class NpcFactory : CharacterFactory
    {
        public override Character CreateCharacter(string name)
        {
            return new Npc(name);
        }
    }
}