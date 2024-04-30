using Graphite.GraphCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;


namespace Graphite.Menu
{
    public class GraphUI
    {
        private OurGraph<char> graph;

        private HashSet<char> nodesAdded;

        public GraphUI()
        {
            graph = new OurGraph<char>(true);
            nodesAdded = new HashSet<char>();
        }

        private void mainMenu()
        {
            ConsoleMenu mainMenu = GenerateMenus.MainMenu();
            string choice = mainMenu.Run();
        }

        private void AddNode()
        {
            Utils.displayList(graph.Values());
            char addNode = Utils.GetChar("Enter a unique node to add to the Graph or 'q' to go back: ", out bool stop);
            if (stop)
            {
                WriteLine("[ Going back to the Main Menu... ]");
                mainMenu();
            }
            else if (nodesAdded.Contains(addNode))
            {
                WriteLine($"[!] The Graph already contains the '{addNode}' Node [!]");
                AddNode();
            }
            else
            {
                WriteLine($"[*] Adding {addNode} to the Graph.. [*]");
                graph.AddNode(addNode);
                if (GenerateMenus.ConfirmMenu("Add Another Node"))
                {
                    AddNode();
                }
            }
        }








    }
}
