using Graphite.GraphCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphite.Menu
{
    public static class GenerateMenus
    {
        public static ConsoleMenu MainMenu()
        {
            string prompt = "Select an Option For the Graph";
            List<string> options = new List<string>
            {
                "Add Node",
                "Add Edge",
                "Remove Node",
                "Remove Edge",
                "Graph Detials",
                "Run Dijkstras",
                "Exit"
            };
            return new ConsoleMenu(options, prompt);
        }

        public static ConsoleMenu GraphInfo(string prompt, OurGraph<char> aGraph,
        ConsoleMenu previous = null)
        {
            var options = aGraph.Values();

            if (previous == null)
            {
                return new ConsoleMenu(options, prompt);
            }

            return new ConsoleMenu(options, prompt, previous);
        }

        public static ConsoleMenu Details(ConsoleMenu MainMenu)
        {
            string prompt = "Select an option to view for the Graph";
            List<string> options = new List<string>()
            {
                "Table View",
                "Directed View",
                "Select a Node to View",
                "Topological Sorts",
            };
            return new ConsoleMenu(options, prompt, MainMenu);
        }

        public static bool ConfirmMenu(string prompt)
        {
            var getChoice = new ConsoleMenu(new List<string> { "YES", "NO" }, prompt);
            return getChoice.Run() == "YES";
        }
    }


    public static class Handlers
    {
        //"Add Node",
        //"Add Edge",
        //"Remove Node",
        //"Remove Edge",
        //"Graph Detials",
        //"Run Dijkstras",
        //"Exit"

    }
}
