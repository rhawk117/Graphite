using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphite.GraphCode
{
    public class Dijkstra<T> where T : IComparable<T>
    {
        private OurGraph<T> _graph;

        private Dictionary<Vertex<T>, int> _distance;

        private Dictionary<Vertex<T>, Vertex<T>> _predecessor;

        private Vertex<T> _currentNode;

        public Dijkstra(OurGraph<T> graph)
        {
            _graph = graph;
            _distance = new Dictionary<Vertex<T>, int>();
            _predecessor = new Dictionary<Vertex<T>, Vertex<T>>();
        }

        public void Run(T startNode)
        {
            Vertex<T> source = _graph.Vertices.Find(v => v.Data.Equals(startNode));
            if (source == null)
            {
                throw new ArgumentException($"Node {startNode} not found in the graph.");
            }

            InitializeSingleSource(source);

            List<Vertex<T>> unvisitedNodes = new List<Vertex<T>>(_graph.Vertices);

            while (unvisitedNodes.Count > 0)
            {
                _currentNode = FindMinDistance(unvisitedNodes);
                unvisitedNodes.Remove(_currentNode);

                foreach (Edge<T> edge in _currentNode.OutEdges)
                {
                    Vertex<T> v = edge.To;
                    int weight = (int)edge.Weight;
                    int distanceThroughU = _distance[_currentNode] + weight;

                    if (!_distance.ContainsKey(v) || distanceThroughU < _distance[v])
                    {
                        _distance[v] = distanceThroughU;
                        _predecessor[v] = _currentNode;
                    }
                }
                DisplayStep();
            }
            FinalResult();
        }

        private void DisplayStep()
        {
            Console.Clear();
            Console.WriteLine(_graph);
            Console.WriteLine();
            Console.WriteLine($"Current Node: {_currentNode.Data}");
            Console.WriteLine("* * * * * * * Table * * * * * * *");
            RenderTable();
            Console.WriteLine("[ Press ENTER to continue... ]");
            Console.ReadLine();
        }

        private void RenderTable()
        {
            int maxVertexLength = _graph.Vertices
                                        .Max(v => v.Data.ToString().Length);
            int vertexPadding = maxVertexLength + 2;

            Console.WriteLine("{0,-" + vertexPadding + "} {1,10} {2,-" + vertexPadding + "}", "Vertex", "Distance", "Predecessor");
            Console.WriteLine(new string('-', vertexPadding * 2 + 20));

            foreach (Vertex<T> vertex in _graph.Vertices)
            {
                T predecessorData = GetPredecessorData(vertex);
                string vertexLine = String.Format("{0,-" + vertexPadding + "} {1,10} {2,-" + vertexPadding + "}", vertex.Data, _distance[vertex], predecessorData);

                if (vertex.Equals(_currentNode))
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(vertexLine);
                Console.ResetColor();
            }
        }

        private Vertex<T> FindMinDistance(List<Vertex<T>> unvisitedNodes)
        {
            Vertex<T> minNode = null;
            int minDistance = int.MaxValue;

            foreach (Vertex<T> node in unvisitedNodes)
            {
                if (_distance[node] < minDistance)
                {
                    minDistance = _distance[node];
                    minNode = node;
                }
            }
            return minNode;
        }

        private void InitializeSingleSource(Vertex<T> source)
        {
            foreach (Vertex<T> vertex in _graph.Vertices)
            {
                _distance[vertex] = int.MaxValue;
                _predecessor[vertex] = default;
            }

            _distance[source] = 0;
        }

        private T GetPredecessorData(Vertex<T> vertex)
        {
            if (_predecessor[vertex] != null && _predecessor.ContainsKey(vertex))
            {
                return _predecessor[vertex].Data;
            }
            else
            {
                return default;
            }
        }

        private void FinalResult()
        {
            Console.Clear();
            Console.WriteLine("\t\t>> Final Result <<");
            RenderTable();
            Console.ReadLine();
        }
    }
}
