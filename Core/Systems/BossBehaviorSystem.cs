using GameEngine2D.Bosses;
using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Core.Systems
{
    public class BossBehaviorSystem
    {
        private const float BossAttackInterval = 3.0f;
        private const float BossAttackVisualDuration = 0.35f;
        private const int BossAttackRange = 5;

        private float bossAttackTimer;
        private float bossAttackVisualTimer;

        public bool IsBossAttacking { get; private set; }

        public void Reset()
        {
            bossAttackTimer = 0f;
            bossAttackVisualTimer = 0f;
            IsBossAttacking = false;
        }

        public string? Update(
            IBoss boss,
            bool isBossSpawned,
            Player player,
            int bossX,
            int bossY,
            float elapsedSeconds,
            out bool playerWasHit)
        {
            playerWasHit = false;

            if (!isBossSpawned || boss.IsDefeated)
            {
                IsBossAttacking = false;
                return null;
            }

            bossAttackTimer += elapsedSeconds;

            if (bossAttackVisualTimer > 0f)
            {
                bossAttackVisualTimer -= elapsedSeconds;

                if (bossAttackVisualTimer <= 0f)
                {
                    IsBossAttacking = false;
                }
            }

            if (bossAttackTimer < BossAttackInterval)
            {
                return null;
            }

            bossAttackTimer = 0f;

            bool playerInRange =
                player.Y == bossY &&
                Math.Abs(player.X - bossX) <= BossAttackRange;

            if (!playerInRange)
            {
                return null;
            }

            IsBossAttacking = true;
            bossAttackVisualTimer = BossAttackVisualDuration;

            player.TakeDamage(boss.Damage);
            playerWasHit = true;

            return boss.Attack();
        }
    }
}