using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Spawning
{
    public abstract class CharacterFactory
    {
        public abstract Character CreateCharacter(string name);

        public Character SpawnCharacter(string name)
        {
            return CreateCharacter(name);
        }
    }
}