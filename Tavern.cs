using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleQuest
{
    class Tavern
    {
        // Responsible for keeping track of the heroes
        int gold = 0;
        public List<Hero> heroes = new List<Hero>();

        // Take in a series of heroes, integrate it into the system!
        public Tavern()
        {
            heroes.Add(new Hero());
            Load();
        }

        // For each hero, if a hero has no hitpoints, restore the individual hero
        public void HealHeroes()
        {
            for (int i = 0; i < heroes.Count; ++i)
            {
                // If a hero is encountered, check their health
                if (heroes[i] != null)
                {
                    if (heroes[i].hitpoints <= 0)
                        heroes[i].hitpoints = heroes[i].maxHitpoints;
                }
                    
            }
        }

        // Display the setup, prompt the user for the hero
        public Hero GetHero()
        {
            HealHeroes();
            Save();

            int choice;
            string input;
            // Continue prompting as long as the input is not a number and out of bounds, and if no new hero has been created
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Tavern.  Choose a Hero.");

                // For each hero, display its information if it's present
                for (int i = 0; i < heroes.Count; ++i)
                {
                    if (heroes[i] != null)
                        Console.WriteLine((i + 1) + ") " + heroes[i].ToString());
                }

                Console.WriteLine((heroes.Count + 1) + ") Create a New Hero");  // Give user the option to create the new hero
                Console.WriteLine((heroes.Count + 2) + ") Quit the Game");  // Display the last option
                input = Console.ReadLine();

                // Reset, prompt again if a new hero is desired
                if (int.Parse(input) == heroes.Count + 1)
                {
                    // Create new hero, integrate into the list, reset display prompt
                    Hero newHero = new Hero();
                    CreateHero(newHero);
                    heroes.Add(newHero);
                }
            }
            while (!int.TryParse(input, out choice) || choice <= 0 || choice > heroes.Count + 2);
                  
            // Check if user wants to exit the game
            if (choice == heroes.Count + 2)
                return null;
            
            return heroes[choice - 1];
        }
        // Prompt user for Hero
        public void CreateHero(Hero hero)
        {
            Console.Write("Enter a name: ");
            hero.name = Console.ReadLine();
        }
        // Milestone 1
        public void Load()
        {
            // If there's no save file, create the file before examination
            if (!File.Exists("/Users/tarikkrestalica/Projects/projectFinal/projectFinal/save.txt"))
                Save();

            FileStream file = File.OpenRead("/Users/tarikkrestalica/Projects/projectFinal/projectFinal/save.txt");
            StreamReader reader = new StreamReader(file);

            // Bypass the first two lines of the file
            string line;
            line = reader.ReadLine();  
            line = reader.ReadLine();  
            gold = int.Parse(line);

            // For each hero, integrate it into the system
            for(int i = 0; i < heroes.Count; ++i)
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (line == "null")
                    {
                        continue;
                    }
                    else if (line == "Hero")
                    {
                        heroes[i] = new Hero();
                    }

                    heroes[i].Load(reader);
                }
            }
            // Save the file
            reader.Close();
            file.Close();
        }
        
        public void Save()
        {
            // Create the empty file, fill it as I go
            FileStream file = File.Create("/Users/tarikkrestalica/Projects/projectFinal/projectFinal/save.txt");
            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine("Save file format version 1.1.0");
            writer.WriteLine(gold);   // Track the gold collected

            // Examine the heroes list, integrate it into the system
            for (int i = 0; i < heroes.Count; ++i)
            {
                if (heroes[i] == null)
                    writer.WriteLine("null");
                else
                    heroes[i].Save(writer);
            }

            writer.Close();
            file.Close();
        }
    }
}

