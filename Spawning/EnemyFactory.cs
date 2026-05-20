using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Spawning
{
    public class EnemyFactory : CharacterFactory
    {
        public override Character CreateCharacter(string name)
        {
            return new Enemy(name, 60, 15, 10, 8);
        }
    }
}