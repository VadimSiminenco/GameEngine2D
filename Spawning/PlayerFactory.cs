using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Spawning
{
    public class PlayerFactory : CharacterFactory
    {
        public override Character CreateCharacter(string name)
        {
            return new Player(name);
        }
    }
}