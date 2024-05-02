using Graphite.GraphCode;
using System;
using System.Collections.Generic;

namespace Graphite.Menu
{
    public static class GenerateMenus
    {
        public static ConsoleMenu MainMenu()
        {
            string prompt = "Select an Option For the Graph";
            List<string> options = new List<string>
            {
                "Add Node", "Add Edge",
                "Remove Node", "Remove Edge",
                "Graph Detials","Run Dijkstras",
                "Reset Graph","Help", "Exit"
            };
            return new ConsoleMenu(options, prompt);
        }
        public static string Custom(string prompt, List<string> options, bool hasBack = false)
        {
            var menu = new ConsoleMenu(options, prompt);
            if (hasBack)
            {
                menu.AddBack();
            }
            return menu.Run();
        }

        // Graph Detaills
        public static ConsoleMenu Details()
        {
            string prompt = "Select an option to view for the Graph";
            List<string> options = new List<string>()
            {
                "Table View",
                "Directed View",
                "Select a Node to View",
                "Topological Sorts",
                "Node Info",
                "Go Back"
            };

            return new ConsoleMenu(options, prompt);
        }
        // Confirm User Choice
        public static bool ConfirmMenu(string prompt)
        {
            var getChoice = new ConsoleMenu(new List<string> { "YES", "NO" }, prompt);
            return getChoice.Run() == "YES";
        }
    }
}
