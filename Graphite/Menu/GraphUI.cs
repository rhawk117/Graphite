using Graphite.GraphCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static System.Console;


namespace Graphite.Menu
{
    public class GraphUI
    {
        private OurGraph<string> graph;

        private HashSet<string> nodesAdded;

        private ConsoleMenu mainMenu;
        public GraphUI()
        {
            graph = new OurGraph<string>(true);
            nodesAdded = new HashSet<string>();
            mainMenu = GenerateMenus.MainMenu();
        }

        public void Run()
        {
            ConsoleMenu mainMenu = GenerateMenus.MainMenu();
            string choice = mainMenu.Run();
            handleMainMenu(choice);
            if (choice != "Exit")
            {
                handleMainMenu(choice);
            }
            else
            {
                Prompt.Info("Thank you for using Graphite I hope you found it useful");
                Environment.Exit(0);
            }
        }

        private void handleMainMenu(string input)
        {
            switch (input)
            {
                case "Add Node":
                    addNode();
                    break;

                case "Add Edge":
                    addEdge();
                    break;

                case "Remove Node":
                    removeNode();
                    break;

                case "Remove Edge":
                    removeEdge();
                    break;

                case "Graph Detials":
                    detailsMenu();
                    break;

                case "Reset Graph":
                    handleReset();
                    break;

                case "Run Dijkstras":
                    handleDijkstras();
                    break;

                case "Help":
                    runHelp();
                    break;

                default:
                    Prompt.Error("Something went horribly wrong...");
                    break;
            }

            Prompt.Wait();
            Run();
        }
        private void runHelp()
        {
            Prompt.DelayMsg("This program is a graph visualizer meant learning graphs easier", 3000);
            Prompt.DelayMsg("The Type for the Nodes/Vertices in the Graph is string for simplicity", 3000);
            Prompt.DelayMsg("Edges are what connect Nodes together; Nodes are what hold the value", 3000);
            Prompt.DelayMsg("To view your graph select > Graph Details", 3000);
            Prompt.DelayMsg("To Add a Node/Edge select > Add Node/Edge", 3000);
            Prompt.DelayMsg("To remove a Node/Edge select > Remove Node/Edge", 3000);
            Prompt.DelayMsg("To run Dijkstra's algorithm you'll need to have a graph with nodes and edges", 3000);
            Prompt.DelayMsg("To reset the graph select > Reset Graph", 3000);
            Prompt.DelayMsg("If you find any bugs or issues let me know and I'll do my best to address them in a timely manner.", 3000);
            Prompt.Info("TUROIAL OVER");
            Prompt.Wait();
        }



        // add methods
        private void addNode()
        {
            Clear();
            Prompt.Info("Nodes In Graph");
            Utils.displayList(graph.Values());
            string addNode = Utils.GetInput("Enter a unique node to add to the Graph or 'q' to go back: ", out bool stop);
            if (stop)
            {
                Prompt.Info("Going back to the Main Menu...");
                Run();
            }
            else if (nodesAdded.Contains(addNode))
            {
                Prompt.Error($"The Graph already contains the '{addNode}' Node ");
                this.addNode();
            }
            else
            {
                WriteLine($"[*] Adding {addNode} to the Graph.. [*]");
                graph.AddNode(addNode);
                if (GenerateMenus.ConfirmMenu("Add Another Node"))
                {
                    this.addNode();
                }
            }
        }
        private void addEdge()
        {
            Clear();
            List<string> allNodes = graph.Values();
            string info = Utils.EdgeInfo();
            string prompt = $"What is the source Node of the edge{info}";
            string fromNode = GenerateMenus.Custom(prompt, allNodes, hasBack: true);
            if (fromNode == "Go Back")
            {
                Run();
            }
            else
            {
                getToEdge(fromNode, allNodes);
            }
        }

        private void getToEdge(string from, List<string> nodes)
        {
            removeExistingEdges(nodes, from);
            nodes.Remove(from);
            string prompt = $"What Node is the edge going to{Utils.EdgeInfo(from)}";
            string toNode = GenerateMenus.Custom(prompt, nodes);
            if (toNode != "Go Back")
            {
                handleWeight(from, toNode);
            }
            else
            {
                Prompt.Info("Going Back...");
                addEdge();
            }
        }
        private void removeExistingEdges(List<string> allNodes, string fromNode)
        {
            var graphNode = graph.GetNode(fromNode);
            foreach (var edge in graphNode.OutEdges)
            {
                allNodes.Remove(edge.To.Data.ToString()); // remove all nodes that have an edge going out of the current node to avoid duplicates
            }
        }
        private void handleWeight(string from, string to)
        {
            WriteLine(Utils.EdgeInfo(from, to));
            double weight = Utils.GetIntput("Provide a weight for your edge or type 'n' if it doesn't have one");
            if (weight == -1)
            {
                Prompt.Info($"Adding Edge From {from} to {to} with no weight");
                graph.AddEdge(from, to);
            }
            else
            {
                Prompt.Info($"Adding Edge From {from} to {to} with a weight of {weight}");
                graph.AddEdge(from, to);
            }
            if (GenerateMenus.ConfirmMenu("Add Another Edge"))
            {
                addEdge();
            }
        }
        // remove methods
        private void removeNode()
        {
            Clear();
            List<string> nodes = graph.Values();
            string prompt = "Select a Node to remove from the Graph";
            string removeVal = GenerateMenus.Custom(prompt, nodes, hasBack: true);
            if (removeVal == "Go Back")
            {
                cancelRemove();
            }
            else
            {
                Prompt.Info($"Removing Node {removeVal} from the Graph");
                graph.RemoveNode(removeVal);
                nodesAdded.Remove(removeVal);
                if (GenerateMenus.ConfirmMenu("Remove Another Node"))
                {
                    removeNode();
                }
            }
        }
        private void removeEdge()
        {
            Clear();
            string prompt = "What is the source Node of the Edge";
            string from = GenerateMenus.Custom(prompt, graph.Values(), hasBack: true);
            if (from == "Go Back")
            {
                cancelRemove();
            }
            else
            {
                var edges = getOutgoingEdges(graph.GetNode(from));

                if (edges == null) return;

                string to = GenerateMenus.Custom("What outgoing edge do you want to remove", edges);
                Prompt.Info($"Removing the following edge in the Graph {Utils.EdgeInfo(from, to)}");
                graph.RemoveEdge(from, to);
                if (GenerateMenus.ConfirmMenu("Remove Another Edge"))
                {
                    removeEdge();
                }
            }
        }

        private List<string> getOutgoingEdges(Node<string> aNode)
        {
            if (aNode.OutEdges.Count == 0)
            {
                Prompt.Error("No Outgoing Edges Found for this Node");
                return null;
            }
            List<string> edges = new List<string>();
            foreach (var edge in aNode.OutEdges)
            {
                edges.Add(edge.To.Data.ToString());
            }
            return edges;
        }

        private void cancelRemove()
        {
            Prompt.Info("Cancelling Remove, Going back to Main Menu...");
            Prompt.Wait();
            Run();
        }
        // misc methods
        private void handleReset()
        {
            if (GenerateMenus.ConfirmMenu("Are you sure you want to reset the Graph"))
            {
                graph.Clear();
                nodesAdded = new HashSet<string>();
                Prompt.Info("Graph has been reset to its default state");
            }
            else
            {
                Prompt.Info("Graph Reset Cancelled");
            }
        }


        private void handleDijkstras()
        {
            string origin = GenerateMenus.Custom("Select a Node to start Dijkstras from", graph.Values(), hasBack: true);
            if (origin == "Go Back")
            {
                Run();
            }
            else
            {
                Dijkstra<string> dijkstra = new Dijkstra<string>(graph);
                dijkstra.Run(origin);
            }
        }
        private void detailsMenu()
        {
            if (graph.IsEmpty())
            {
                Prompt.Error("Your Graph is Looking a little empty.. Nothing to view");
                Prompt.Wait();
            }
            else
            {
                ConsoleMenu detailsMenu = GenerateMenus.Details();
                string choice = detailsMenu.Run();
                if (choice != "Go Back")
                {
                    handleDetails(choice);
                }
                else
                {
                    Run();
                }
            }
        }

        private void handleDetails(string input)
        {
            Prompt.Info($"Displaying => {input}");
            Prompt.Wait();
            switch (input)
            {
                case "Table View":
                    tableView();
                    break;

                case "Directed View":
                    directedView();
                    break;

                case "Select a Node to View":
                    selectNode();
                    break;

                case "Topological Sorts":
                    topologicalSorts();
                    break;

                case "Node Info":
                    displayNodeInfo();
                    break;

                default:
                    Prompt.Error("Something went horribly wrong...");
                    break;

            }
            Prompt.Wait();
            detailsMenu();
        }

        private void tableView()
        {
            Prompt.Info("Displaying table of Graph");
            graph.DisplayGraphTable();
            Prompt.Wait();
        }

        private void directedView()
        {
            Prompt.Info("Starting Directed View for Graph");
            graph.DirectedView();
            Prompt.Wait();
        }

        private void selectNode()
        {
            string prompt = "Select a Node in the Graph to view";
            string choice = GenerateMenus.Custom(prompt, graph.Values(), hasBack: true);
            if (choice == "Go Back")
            {
                detailsMenu();
            }
            else
            {
                Prompt.Info($"Fetching information for Node {choice}");
                graph.VertexDetails(graph.GetNode(choice));
                Prompt.Wait();
            }
        }
        private void topologicalSorts()
        {
            Prompt.Info("Fetching all Valid Topological Sorts...");
            var sorts = graph.GetAllTopologicalSorts().ToList();
            if (sorts.Count == 0)
            {
                Prompt.Error("No Topological Sorts Were Found In Your Graph");
            }
            else
            {
                Prompt.Info("Displaying all Topological Sorts...");
                foreach (var sort in sorts)
                {
                    Utils.displayList(sort);
                    WriteLine();
                }
                Prompt.Wait();
            }
        }
        private void displayNodeInfo()
        {
            Prompt.Info("Displaying Node Information");
            WriteLine(graph.NodeInfo());
            Prompt.Wait();
        }
    }
}
