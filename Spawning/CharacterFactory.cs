using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Spawning
{
    public abstract class CharacterFactory
    {
        public abstract Character CreateCharacter(string name);

        public Character SpawnCharacter(string name)
        {
            Character character = CreateCharacter(name);

            Console.WriteLine($"[Factory Method] {character.GetType().Name} \"{character.Name}\" spawned at ({character.X},{character.Y})");

            return character;
        }
    }
}