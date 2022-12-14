using System;
using System.Collections.Generic;

namespace ConsoleQuest
{
    class Program
    {
        private List<AdventureEvent> events = new List<AdventureEvent>();

        static void Main(string[] args)
        {
            // Create the Tavern, have the tavern display the heroes
            Tavern tavern = new Tavern();
            Hero player = tavern.GetHero();

            // Play the game until no player is specified
            while (player != null)
            {
                // Set the player onto an adventure, load up the events
                Adventure adv = new Adventure(player);
                adv.LoadEvents(@"/Users/tarikkrestalica/Projects/projectFinal/projectFinal/events.txt");
                adv.Begin();

                // While the player has not completed the adventure, choose a random event
                while (!adv.done)
                {
                    adv.TryEvent();
                }

                // After the event, choose another hero
                player = tavern.GetHero();
            }
        }
    }
}

