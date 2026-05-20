using GameEngine2D.Entities.Characters;
using GameEngine2D.Entities.Structures;

namespace GameEngine2D.Core.Systems
{
    public class InteractionSystem
    {
        public bool IsPlayerOnTrap(Player player, List<Structure> structures)
        {
            return structures.Any(structure =>
                structure.Name == "Trap" &&
                structure.X == player.X &&
                structure.Y == player.Y);
        }

        public bool TryActivateTrap(Player player, List<Structure> structures, int damage)
        {
            Structure? trap = structures.FirstOrDefault(structure =>
                structure.Name == "Trap" &&
                structure.X == player.X &&
                structure.Y == player.Y);

            if (trap == null)
            {
                return false;
            }

            player.TakeDamage(damage);
            structures.Remove(trap);

            return true;
        }
    }
}