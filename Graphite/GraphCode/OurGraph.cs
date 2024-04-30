using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Spectre.Console;



namespace Graphite.GraphCode
{
    public class OurGraph<T> where T : IComparable<T>
    {
        private bool Directional = false;

        private List<Node<T>> _mNodes;

        // typically read-only but need to use for dijkstra's class
        public List<Node<T>> Nodes
        {
            get => _mNodes;
        }

        public int Count
        {
            get => Nodes.Count;
        }


        public OurGraph(bool isDirectional = false, int size = 20)
        {
            Directional = isDirectional;

            if (size < 20)
            {
                size = 20;
            }
            _mNodes = new List<Node<T>>(size);
        }

        public void RemoveEdge(T source, T dest)
        {
            // get src and dest
            Node<T> sourceNode = _mNodes.
                                    Find(g => g.Data.Equals(source));

            Node<T> destinationNode = _mNodes.
                                        Find(g => g.Data.Equals(dest));

            for (int i = 0; i < sourceNode.OutEdges.Count; i++)
            {
                if (sourceNode.OutEdges[i].From.Data.Equals(source) &&
                    sourceNode.OutEdges[i].To.Data.Equals(dest)
                )
                {
                    sourceNode.OutEdges.RemoveAt(i);
                }
            }
            // if bidirectional, remove edge going in other direction
            if (Directional == false)
            {
                for (int i = 0; i < sourceNode.InEdges.Count; i++)
                {
                    if (sourceNode.InEdges[i].From.Data.Equals(dest) &&
                        sourceNode.InEdges[i].To.Data.Equals(source)
                    )
                    {
                        sourceNode.InEdges.RemoveAt(i);
                    }
                }

                for (int i = 0; i < destinationNode.OutEdges.Count; i++)
                {
                    if (destinationNode.OutEdges[i].From.Data.Equals(dest) &&
                        destinationNode.OutEdges[i].To.Data.Equals(source)
                    )
                    {
                        destinationNode.OutEdges.RemoveAt(i);
                    }
                }

                for (int i = 0; i < destinationNode.InEdges.Count; i++)
                {
                    if (destinationNode.InEdges[i].From.Data.Equals(source) &&
                        destinationNode.InEdges[i].To.Data.Equals(dest)
                    )
                    {
                        destinationNode.InEdges.RemoveAt(i);
                    }
                }
            }
        }

        public void AddEdge(T source, T dest, int aWeight = 1)
        {
            // if src or dest not in graph add them 
            if (TryFind(ref source) == false)
            {
                AddNode(source);
            }

            if (TryFind(ref dest) == false)
            {
                AddNode(dest);
            }

            // add dest to source's adjacency list
            Node<T> sourceNode = _mNodes
                                    .Find(g => g.Data.Equals(source) == true);

            Node<T> destinationNode = _mNodes
                                        .Find(g => g.Data.Equals(dest) == true);

            if (sourceNode != null && destinationNode != null)
            {
                Edge<T> newEdge = new Edge<T>(sourceNode, destinationNode, aWeight);
                sourceNode.OutEdges.Add(newEdge);
                destinationNode.InEdges.Add(newEdge);

                // if bidirectional, add to source's incoming and destination's outgoing edges
                if (Directional == false)
                {
                    Edge<T> otherDirectionEdge = new Edge<T>(destinationNode, sourceNode, aWeight);
                    sourceNode.InEdges.Add(otherDirectionEdge);
                    destinationNode.OutEdges.Add(otherDirectionEdge);
                }
            }
        }

        public bool AddNode(T value)
        {
            // Don't add if already in Graph
            if (_mNodes.Find(g => g.Data.CompareTo(value) == 0) == null)
            {
                _mNodes.Add(new Node<T>(value));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool RemoveNode(Node<T> aVertex)
        {
            if (aVertex == null || _mNodes.FindIndex(tmp => tmp == aVertex) == -1)
            {
                return false;
            }

            for (int j = 0; j < aVertex.OutEdges.Count; j++)
            {
                int indexEdge = aVertex.OutEdges[j].To.InEdges
                                .FindIndex(e => e.From.Equals(aVertex));
                if (indexEdge != -1)
                {
                    aVertex.OutEdges[j].To.InEdges.RemoveAt(indexEdge);
                }
            }
            _mNodes.Remove(aVertex);
            return true;
        }

        public bool RemoveNode(T value)
        {
            Node<T> pDel = _mNodes.Find(g => g.Data.CompareTo(value) == 0);
            if (pDel != null)
            {
                for (int j = 0; j < pDel.OutEdges.Count; j++)
                {
                    int indexEdge = pDel.OutEdges[j].To.InEdges
                                    .FindIndex(e => e.From.Equals(pDel));
                    if (indexEdge != -1)
                    {
                        pDel.OutEdges[j].To.InEdges.RemoveAt(indexEdge);
                    }
                }

                _mNodes.Remove(pDel); // remove node
                return true;
            }
            return false;
        }

        public bool Contains(T value) => _mNodes.Find(n => n.Data.CompareTo(value) == 0) == null;

        public bool TryFind(ref T value)
        {
            T tmpValue = value;
            Node<T> tmp = _mNodes.Find(g => g.Data.CompareTo(tmpValue) == 0);
            if (tmp == null)
            {
                return false;
            }
            else
            {
                value = tmpValue;
                return true;
            }
        }
        public bool IsEmpty() => Nodes.Count == 0;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Node<T> vertices in _mNodes)
            {
                sb.Append("Node: " + vertices);
                if (vertices.OutEdges.Count != 0)
                {
                    sb.Append("\tNeighbors are ");
                    foreach (var x in vertices.OutEdges)
                    {
                        sb.Append(x.To.Data.ToString() + ", ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public void DirectedView()
        {
            if (Directional == false)
            {
                Console.WriteLine("Graph is not directed");
                return;
            }

            foreach (Node<T> vertex in _mNodes)
            {
                Console.WriteLine($"*---------------({vertex.Data})---------------*");
                VertexDetails(vertex);
            }
        }

        private void VertexDetails(Node<T> vertice)
        {
            Console.WriteLine($"[o] Num. of Outgoing Edges = {vertice.OutEdges.Count} [o]");
            Console.WriteLine("<format> - (FROM) ---[weight]---> (TO)");

            foreach (Edge<T> edge in vertice.OutEdges)
            {
                Console.WriteLine($"\n({edge.From.Data}) ---[{edge.Weight}]---> ({edge.To.Data})\n");
                Utils.awaitEnter();
            }

            Utils.Line();
            Console.WriteLine($"\n[i] Num. of Incoming Edges => ({vertice.InEdges.Count}) [i]");

            Console.WriteLine("(TO) <---[weight]--- (FROM)");


            foreach (Edge<T> edge in vertice.InEdges)
            {
                Console.WriteLine($"\n({edge.To.Data}) <---[{edge.Weight}]--- ({edge.From.Data})\n");
                Utils.awaitEnter();
            }
            Utils.Line();
        }
        public void GetTopologicalSorts()
        {
            HashSet<T> visited = new HashSet<T>();
            Stack<T> stack = new Stack<T>();
            List<List<T>> allSorts = new List<List<T>>();

            // Helper method for DFS-based topological sort
            void AllTopoSortsDFS(Node<T> vertex)
            {
                visited.Add(vertex.Data);
                stack.Push(vertex.Data);

                // Explore each adjacent vertex
                foreach (var edge in vertex.OutEdges)
                {
                    if (!visited.Contains(edge.To.Data))
                    {
                        AllTopoSortsDFS(edge.To);
                    }
                }

                // If all vertices are visited, we have a complete sort
                if (visited.Count == Nodes.Count)
                {
                    allSorts.Add(new List<T>(stack.Reverse()));
                }

                // Backtrack
                stack.Pop();
                visited.Remove(vertex.Data);
            }

            // Initial call to the recursive function for all
            // vertices not in a cycle
            foreach (var vertex in Nodes)
            {
                if (!visited.Contains(vertex.Data))
                {
                    AllTopoSortsDFS(vertex);
                }
            }

            // Displaying all topological sorts
            foreach (var sort in allSorts)
            {
                Console.WriteLine(string.Join(" -> ", sort));
            }
        }

        public string DisplayVertices()
        {
            if (Nodes.Count == 0)
            {
                return "[ NO VERTICES ]";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append($"Num. Vertices => {Nodes.Count}");
            foreach (Node<T> vertex in Nodes)
            {
                sb.Append($" ({vertex.Data}) ");
            }
            return sb.ToString();
        }

        public void DisplayGraphTable()
        {
            var table = new Table();

            // Add columns to the table for the node, outgoing edges, and incoming edges
            table.AddColumn(new TableColumn("[u]Node[/]").Centered());
            table.AddColumn(new TableColumn("[u]Outgoing Edges (To:Weight)[/]").Centered());
            table.AddColumn(new TableColumn("[u]Incoming Edges (From:Weight)[/]").Centered());

            foreach (Node<T> node in _mNodes)
            {
                // Prepare the outgoing edges string
                var outgoing = node.OutEdges.Count > 0
                    ? string.Join(", ", node.OutEdges.Select(e => $"{e.To.Data}({e.Weight})"))
                    : "None";

                // Prepare the incoming edges string
                var incoming = node.InEdges.Count > 0
                    ? string.Join(", ", node.InEdges.Select(e => $"{e.From.Data}({e.Weight})"))
                    : "None";

                // Add a row for each node in the graph
                table.AddRow(node.Data.ToString(), outgoing, incoming);
            }

            // Set table design
            table.Border(TableBorder.Rounded);
            table.Title("[[ Graph Nodes Overview ]]");
            table.Caption("Displayed are the nodes and their corresponding edges in the graph.");

            // Render the table to the console
            AnsiConsole.Write(table);
        }

        public List<string> Values()
        {
            var values = new List<string>();
            foreach (Node<T> vertex in _mNodes)
            {
                values.Add(vertex.Data.ToString());
            }
            return values;
        }

        public IEnumerable<List<T>> GetAllTopologicalSorts()
        {
            var visited = new HashSet<Node<T>>();
            var stack = new Stack<Node<T>>();
            var result = new List<List<T>>();

            foreach (var node in Nodes)
            {
                if (!visited.Contains(node))
                {
                    if (!TopologicalSort(node, visited, stack, result))
                    {
                        // If the graph contains a cycle, clear the result list and return
                        result.Clear();
                        return result;
                    }
                }
            }

            return result;
        }

        private bool TopologicalSort(Node<T> node, HashSet<Node<T>> visited, Stack<Node<T>> stack, List<List<T>> result)
        {
            visited.Add(node);
            stack.Push(node);

            foreach (var edge in node.OutEdges)
            {
                if (!visited.Contains(edge.To))
                {
                    if (!TopologicalSort(edge.To, visited, stack, result))
                    {
                        return false;
                    }
                }
                else if (stack.Contains(edge.To))
                {
                    // Cycle detected
                    return false;
                }
            }

            var sortedNodes = new List<T>();
            while (stack.Count > 0)
            {
                var n = stack.Pop();
                sortedNodes.Add(n.Data);
            }

            result.Add(sortedNodes);
            return true;
        }


    }

}
