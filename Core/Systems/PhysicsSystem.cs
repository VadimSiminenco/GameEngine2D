using GameEngine2D.Entities.Characters;
using GameEngine2D.Entities.Structures;

namespace GameEngine2D.Core.Systems
{
    public class PhysicsSystem
    {
        public const int CharacterGroundY = 17;
        public const int MinWorldX = 0;
        public const int MaxWorldX = 38;
        private const float GravityStepInterval = 0.12f;

        private float playerGravityTimer;

        public void UpdatePlayerGravity(
            Player player,
            List<Structure> structures,
            float elapsedSeconds)
        {
            int supportY = GetSupportY(player, structures);

            if (player.Y >= supportY)
            {
                player.Y = supportY;
                playerGravityTimer = 0f;
                return;
            }

            playerGravityTimer += elapsedSeconds;

            if (playerGravityTimer >= GravityStepInterval)
            {
                playerGravityTimer = 0f;
                player.Y++;
            }
        }

        public bool IsPlayerOnGroundOrPlatform(Player player, List<Structure> structures)
        {
            int supportY = GetSupportY(player, structures);
            return player.Y >= supportY;
        }

        public int GetStructureGroundY(Structure structure)
        {
            return structure.Name switch
            {
                "House" => 16,
                "Bridge" => 16,
                "Trap" => 17,
                _ => 17
            };
        }

        private int GetSupportY(Player player, List<Structure> structures)
        {
            int supportY = CharacterGroundY;

            foreach (Structure structure in structures)
            {
                if (!IsPlatform(structure))
                {
                    continue;
                }

                int platformTopY = GetPlatformTopY(structure);

                bool playerIsHorizontallyOnPlatform =
                    player.X >= structure.X &&
                    player.X <= structure.X + GetPlatformWidth(structure) - 1;

                bool playerIsAboveOrOnPlatform =
                    player.Y <= platformTopY;

                if (playerIsHorizontallyOnPlatform && playerIsAboveOrOnPlatform)
                {
                    supportY = Math.Min(supportY, platformTopY);
                }
            }

            return supportY;
        }

        private bool IsPlatform(Structure structure)
        {
            return structure.Name == "House" || structure.Name == "Bridge";
        }

        private int GetPlatformTopY(Structure structure)
        {
            return structure.Name switch
            {
                "House" => structure.Y - 1,
                "Bridge" => structure.Y,
                _ => CharacterGroundY
            };
        }

        private int GetPlatformWidth(Structure structure)
        {
            return structure.Name switch
            {
                "House" => 4,
                "Bridge" => 2,
                _ => 1
            };
        }
        public void ClampCharacterToWorld(Character character)
        {
            if (character.X < MinWorldX)
            {
                character.X = MinWorldX;
            }

            if (character.X > MaxWorldX)
            {
                character.X = MaxWorldX;
            }
        }
    }
}