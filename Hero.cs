using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleQuest
{
    class Hero
    {
        // Heroes have a name, hitpoints, and the maximum number of hitpoints
        public string name;
        public int hitpoints;
        public int maxHitpoints;

        // Every hero starts at level 1, with no experience or gold
        public int level = 1;
        public int experience = 0;
        public int gold = 0;

        // Prepare the inventory, create a default hero
        public List<string> inventory = new List<string>();
        public Hero()
        {
            name = "Jon";
            maxHitpoints = 5;
            hitpoints = maxHitpoints;
        }
        // Based on the hero chosen, decrement the hitpoints based on the amount specified
        public void TakeDamage(int amount)
        {
            hitpoints -= amount;
            if (hitpoints <= 0) // Check if the hero's hitpoints reaches 0
                hitpoints = 0;
        }
        // Add to user's inventory
        public void GiveItem(string item)
        {
            inventory.Add(item);
        }

        public bool HasItem(string item)
        {
            for (int i = 0; i < inventory.Count; ++i)
            {
                if (inventory[i] == item)
                    return true;
            }
            return false;
        }
        // Display the hero's information
        public override string ToString()
        {
            return name + ": Level " + level + ", HP=" + hitpoints + ", Items:" + inventory.Count;
        }
        // Responsible for displaying the items the hero has
        public void PrintInventory()
        {
            Console.WriteLine(name + "'s " + "Inventory!");
            for (int i = 0; i < inventory.Count; ++i)
            {
                Console.WriteLine(inventory[i]);
            }
            Console.WriteLine("Press Enter to continue");
        }
        // Does the hero have 10 experience points?
        public bool CheckLevelUp()
        {
            return experience > 10;
        }
        // Attain the information, convert each piece of info to the appropriate data type
        public void Load(StreamReader reader)
        {
            string line;
            line = reader.ReadLine();  // name
            name = line;

            // hitpoints
            line = reader.ReadLine();  
            hitpoints = int.Parse(line);
            // maxHitpoints
            line = reader.ReadLine();  
            maxHitpoints = int.Parse(line);
            // level
            line = reader.ReadLine();  
            level = int.Parse(line);
            // experience
            line = reader.ReadLine();  
            experience = int.Parse(line);
            // gold
            line = reader.ReadLine();  
            gold = int.Parse(line);

            // Attain if the user has any inventory, if so, print out the items
            int numItems = 0;
            line = reader.ReadLine();  
            numItems = int.Parse(line);
            for (int i = 0; i < numItems; ++i)
            {
                inventory.Add(reader.ReadLine());
            }
        }
        // Integrate the hero's information, list out its inventory
        public void Save(StreamWriter writer)
        {
            writer.WriteLine("Hero");
            writer.WriteLine(name);
            writer.WriteLine(hitpoints);
            writer.WriteLine(maxHitpoints);
            writer.WriteLine(level);
            writer.WriteLine(experience);
            writer.WriteLine(gold);
            writer.WriteLine(inventory.Count);
            for (int i = 0; i < inventory.Count; ++i)
            {
                writer.WriteLine(inventory[i]);
            }
        }
    }
}

