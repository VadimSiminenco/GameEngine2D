using System;
using GameEngine2D.Bosses;
using GameEngine2D.Weapons;

namespace GameEngine2D.Facades
{
    public class BattleFacade
    {
        public void StartBossBattle(IWeapon playerWeapon, IBoss boss)
        {
            Console.WriteLine("=== Boss Battle Scene ===");
            Console.WriteLine();

            Console.WriteLine("Before spawn:");
            Console.WriteLine(boss.GetInfo());
            Console.WriteLine();

            boss.Spawn();
            Console.WriteLine();

            Console.WriteLine("After spawn:");
            Console.WriteLine(boss.GetInfo());
            Console.WriteLine();

            Console.WriteLine("Player attacks with weapon:");
            playerWeapon.Use();
            Console.WriteLine();

            Console.WriteLine("Boss counterattacks:");
            boss.Attack();
            Console.WriteLine();

            Console.WriteLine("Battle scene finished.");
        }
    }
}