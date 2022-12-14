using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleQuest
{
    class Adventure
    {
        // Stores the entire game logic, keep track of the currency, and game state
        List<AdventureEvent> events = new List<AdventureEvent>();  
        public int gold = 0;
        public bool done = false; // Individual adventure

        // Create the adventure, integrate the player
        Hero hero;
        public Adventure(Hero newHero)
        {
            hero = newHero;
        }
        // Examine the file from top to bottom, returns whether or not the file has finished loading
        private bool IsEndOfFile(StreamReader reader)
        {
            while (true)
            {
                // Examine the current line
                int peek = reader.Peek();
                if (peek <= -1) // Empty line
                    return true;
                if ((char)peek != '#')  // Event name
                    return false;

                reader.ReadLine();  // Move onto the next line 
            }
        }
        // Examine the text file, incorporate the game logic
        public void LoadEvents(string filename)
        {
            // Bypass the first line
            StreamReader reader = new StreamReader(filename);
            reader.ReadLine();  // version info

            // Until the end of the file is met, create the events
            while (!IsEndOfFile(reader))
            {
                // Given the event name, set it up
                AdventureEvent evt = new AdventureEvent();
                evt.Load(reader);  
                AddEvent(evt);
            }
        }
        // Add the event to the events list(which is defined as AdventureEvent)
        public void AddEvent(AdventureEvent newEvent)
        {
            events.Add(newEvent);
        }
        // Choose a random event from the list 
        public AdventureEvent GetRandomEvent()
        {
            Random rnd = new Random();
            int index = rnd.Next(0, events.Count);
            return events[index];
        }
        // Indicate a started adventure
        public void Begin()
        {
            Console.WriteLine(hero.name + " sets out on a new adventure!");
        }

        public void TryEvent()
        {
            // Refresh the screen, choose a random event, set it up
            Console.Clear();
            AdventureEvent evt = GetRandomEvent();

            // Prompt the user, evaluate their choice
            bool eventDone = false;
            while (!eventDone)
            {
                // Prepare responses
                string input;
                int choice;
                do
                {
                    // Display the hero's information, the event, and the choices entitled to the event
                    Console.WriteLine(hero.ToString());
                    Console.WriteLine(evt.GetDescription(hero));
                    Console.WriteLine(evt.GetChoices(hero));

                    input = Console.ReadLine();
                    Console.Clear();   // Refresh console after input
                }
                // Continue prompting while the input is not a number or if it exceeds the number of options
                while (!int.TryParse(input, out choice) || choice < 0 || choice > evt.GetNumChoices()); 

                // If choice is 0, the hero will display its inventory
                if (choice == 0)
                {
                    hero.PrintInventory();
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    // Indicate a finished event, reward or punish the user
                    eventDone = true;
                    evt.Choose(this, hero, choice);

                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();
                    Console.Clear();

                    // Game over until the player has no health
                    if (hero.hitpoints <= 0 || evt.ShouldEndAdventure())
                    {
                        End(hero);
                    }
                    else
                    {
                        // Otherwise, check in with the user
                        do
                        {
                            Console.WriteLine("Should " + hero.name + " continue forth?");
                            Console.WriteLine("1) Onward!");
                            Console.WriteLine("2) Back to the Tavern");

                            input = Console.ReadLine();
                            Console.Clear();    // After input, refresh the console
                        }
                        // Continue asking until it's a number and it's either 1 or 2
                        while (!int.TryParse(input, out choice) || choice < 1 || choice > 2);

                        if (choice == 2)
                            End(hero);
                    }
                }
            }
        }

        // Summarize the adventure
        private void End(Hero hero)
        {
            // Summary, track the player's progress
            done = true;
            Console.WriteLine(hero.name + " collected " + gold + " gold on the adventure!");
            Console.WriteLine(hero.name + " wakes up refreshed in bed at the Tavern.");
            // Reset gold count to prepare for new adventure
            hero.gold += gold;
            gold = 0;

            Console.ReadLine();
        }
    }
}

