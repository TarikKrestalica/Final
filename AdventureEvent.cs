using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleQuest
{
    class AdventureEvent
    {
        // Keep track of event state
        private bool doneAdventure = false;

        // Every event has its own name, description, series of choices, responses and rewards
        string name;
        string description = "An Event";
        List<string> choices = new List<string>();
        Dictionary<int, string> responses = new Dictionary<int, string>();
        Dictionary<int, List<string>> rewards = new Dictionary<int, List<string>>();

        // Responsible to get all the information for my events
        private string ReadNextLine(StreamReader reader)
        {
            while (reader.Peek() > -1)
            {
                // Get the next line down, ignore any comments(denoted by #)
                string line = reader.ReadLine();
                if (line.Length > 0 && line[0] == '#')  
                    continue;
                return line;
            }
            return string.Empty;
        }

        public void Load(StreamReader reader)
        {
            // Get the event name
            string line;
            line = ReadNextLine(reader);
            name = line;

            // Get description, check if I have more than one part
            int number;
            line = ReadNextLine(reader);  
            if (!int.TryParse(line, out number))
                description = line;

            // If I have more than one, append each part 
            else
            {
                description = string.Empty;
                for (int i = 0; i < number; ++i)
                {
                    line = ReadNextLine(reader);  // description
                    description += line + "\n";
                }
            }

            // Get the number of choices associated with the event
            line = ReadNextLine(reader);  
            number = int.Parse(line);

            // For every choice in my current event
            for (int i = 0; i < number; ++i)
            {
                // Prepare my option, response, and the list of rewards
                string choice = ReadNextLine(reader);  
                string response = string.Empty;
                List<string> rewards = new List<string>();

                // Check if I have more than one part in the response
                int count;   
                line = ReadNextLine(reader);
                if (!int.TryParse(line, out count))
                    response = line;
                else
                {
                    for (int j = 0; j < count; ++j)
                    {
                        line = ReadNextLine(reader);  // response
                        response += line + "\n";
                    }
                }

                // Properly integrate the rewards
                line = ReadNextLine(reader);

                // Check if I have more than one reward
                if (!int.TryParse(line, out count))
                    rewards.Add(line);
                else
                {
                    for (int j = 0; j < count; ++j)
                    {
                        line = ReadNextLine(reader);  
                        rewards.Add(line);
                    }
                }

                AddChoice(choice, response, rewards);
            }
        }

        public void SetDescription(string desc)
        {
            description = desc;
        }
        // Event will get its own description
        public string GetDescription(Hero hero)
        {
            return description;
        }

        public void AddChoice(string choice, string response)
        {
            choices.Add(choice);
            responses.Add(choices.Count, response);
        }
        // Define the event choice by choice(link response and reward based on the length of my choices list)
        public void AddChoice(string choice, string response, List<string> rewards)
        {
            choices.Add(choice);
            responses.Add(choices.Count, response);
            this.rewards.Add(choices.Count, rewards);
        }
        // List all of the choices the hero is entitled to based on the event given
        public string GetChoices(Hero hero)
        {
            string choiceString = "0) Check Inventory\n";
            for (int i = 0; i < choices.Count; ++i)
                choiceString += (i + 1) + ") " + choices[i] + '\n';

            return choiceString;
        }
        // Based on the current event, return the number of options
        public int GetNumChoices()
        {
            return choices.Count;
        }
        
        public void Choose(Adventure adv, Hero hero, int choice)
        {
            // Print out the appropriate response
            if (responses.ContainsKey(choice))
                Console.WriteLine(responses[choice]);

            Console.WriteLine();

            // Give me the appropriate reward
            if (rewards.ContainsKey(choice))
                GiveRewards(adv, hero, rewards[choice]);
        }

        public void GiveRewards(Adventure adv, Hero hero, List<string> rewardList)
        {
            // Go through the list of rewards and events
            for (int i = 0; i < rewardList.Count; ++i)
            {
                string reward = rewardList[i];
                switch (reward)
                {
                    // No rewards are given
                    case "":
                        break;
                    // Did the user explicitly stop the adventure
                    case "doneAdventure":
                        doneAdventure = true;
                        break;
                    // If the hero takes damage, minimize their health points by 1
                    case "damage":
                        hero.TakeDamage(1);
                        Console.WriteLine(hero.name + " took " + 1 + " damage!");
                        break;
                    // Immediate game over
                    case "kill":
                        hero.TakeDamage(10000);
                        Console.WriteLine(hero.name + " has been knocked out!");
                        break;
                    // Does the user encounter the enemy?
                    case "battle":
                        // Create, Start the Battle
                        CombatEvent combatEvent = new CombatEvent(hero, name);
                        combatEvent.InitiateBattle();
                        // If the hero wins, add the gold and xp as rewards
                        if (combatEvent.heroWin)
                        {
                            Console.WriteLine($"Congratulations {hero.name}! 1 gold and xp will be awarded to you!");
                            Console.ReadLine();
                            rewardList.Add("gold");
                            rewardList.Add("xp");
                        }
                        else
                            Console.WriteLine("Game Over!");
                        break;
                    // If I obtain gold, indicate it on my adventure(separate game sessions)
                    case "gold":
                        adv.gold += 1;
                        Console.WriteLine(hero.name + " found " + 1 + " gold!");
                        break;
                    // If I obtain experience, check if the hero can level up
                    case "xp":
                        hero.experience += 1;
                        Console.WriteLine(hero.name + " gained " + 1 + " experience!");
                        if (hero.CheckLevelUp())
                        {
                            hero.experience -= 10;
                            hero.maxHitpoints += 2;
                            hero.level += 1;
                        }
                        break;
                    // If the hero gets an item, add it to the hero's inventory
                    default:
                        hero.GiveItem(reward);
                        Console.WriteLine(hero.name + " got a " + reward + ".");
                        break;
                }
            }
        }
        // Is the user ready to quit the adventure and go back to the tavern?
        public bool ShouldEndAdventure()
        {
            return doneAdventure;
        }
    }
}
