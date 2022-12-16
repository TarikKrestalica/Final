using System;

namespace ConsoleQuest
{
    class CombatEvent
    {
        // Defined by a Battle engaged by the Hero and Enemy
        public Hero hero;
        public string enemy;
        public int enemyHitPoints; // For now, we want to integrate the hero's information

        // Battle information: Track if the hero wins!
        public bool heroWin = false;
 
        public CombatEvent(Hero aHero, string aEnemy)
        {
            hero = aHero;
            enemy = aEnemy;
            enemyHitPoints = hero.maxHitpoints;
        }

        // Engage, Start the Battle
        public void InitiateBattle()
        {
            // Generate random numbers to take away the hero and enemy hitpoints
            Random rnd = new Random();  

            // Continue until either the hero or enemy is dead
            while (true)
            {
                // Prompt for attack, refresh the screen
                Console.WriteLine("Press Enter to launch attack!");
                Console.ReadLine();
                Console.Clear();

                // Player attacks first, check if hero wins!
                int playerDamage = rnd.Next(0, 3);
                enemyHitPoints -= playerDamage;
                if (enemyHitPoints <= 0)
                {
                    heroWin = true;
                    break;
                }

                // Enemy attacks next, check if enemy wins!
                int enemyDamage = rnd.Next(0, 3);
                hero.TakeDamage(enemyDamage);
                if (hero.hitpoints <= 0)
                    break;

                // Provide an update after battle
                Console.WriteLine("After Attack:");
                Console.Write($"{hero.name}: {hero.hitpoints} HP, {enemy}: {enemyHitPoints} HP");
                Console.ReadLine();  
            }

        }
    }
}


