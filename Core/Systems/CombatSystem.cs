using GameEngine2D.Bosses;
using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Core.Systems
{
    public class CombatSystem
    {
        private const int PlayerAttackDamage = 25;
        private const int BossAttackDamage = 25;
        private const int BossHitRange = 4;
        private const float EnemyAttackInterval = 1.0f;

        private float enemyAttackTimer;

        public Enemy? FindEnemyInAttackRange(Player player, List<Enemy> enemies)
        {
            int attackX = player.X + player.FacingDirection;

            return enemies.FirstOrDefault(enemy =>
                enemy.X == attackX &&
                enemy.Y == player.Y);
        }

        public bool TryAttackEnemy(Player player, List<Enemy> enemies)
        {
            Enemy? target = FindEnemyInAttackRange(player, enemies);

            if (target == null)
            {
                return false;
            }

            target.Health -= PlayerAttackDamage;

            if (target.Health <= 0)
            {
                enemies.Remove(target);
            }

            return true;
        }

        public bool TryAttackBoss(
            Player player,
            IBoss boss,
            bool isBossSpawned,
            int bossX,
            int bossY,
            out string bossMessage)
        {
            bossMessage = string.Empty;

            if (!isBossSpawned || boss.IsDefeated)
            {
                return false;
            }

            bool playerIsNearBoss =
                player.Y == bossY &&
                Math.Abs(player.X - bossX) <= BossHitRange;

            if (!playerIsNearBoss)
            {
                return false;
            }

            bool playerLooksAtBoss =
                bossX > player.X && player.FacingDirection > 0 ||
                bossX < player.X && player.FacingDirection < 0;

            if (!playerLooksAtBoss)
            {
                return false;
            }

            bossMessage = boss.TakeDamage(BossAttackDamage);
            return true;
        }

        public bool UpdateEnemyAttacks(Player player, List<Enemy> enemies, float elapsedSeconds)
        {
            enemyAttackTimer += elapsedSeconds;

            if (enemyAttackTimer < EnemyAttackInterval)
            {
                return false;
            }

            Enemy? attackingEnemy = enemies.FirstOrDefault(enemy =>
                enemy.Health > 0 &&
                enemy.Y == player.Y &&
                Math.Abs(enemy.X - player.X) == 1);

            if (attackingEnemy == null)
            {
                return false;
            }

            enemyAttackTimer = 0f;
            player.TakeDamage(attackingEnemy.Damage);

            return true;
        }
    }
}