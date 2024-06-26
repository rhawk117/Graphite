﻿using System;
using System.Collections.Generic;
using Spectre.Console;

namespace Graphite.GraphCode
{
    // it's not perfect but it works most of the time & is best i could come up with
    public class Dijkstra<T> where T : IComparable<T>
    {
        private OurGraph<T> _graph;

        private Dictionary<Node<T>, int> _distance;

        private Dictionary<Node<T>, Node<T>> _predecessor;

        private Node<T> _currentNode;

        public Dijkstra(OurGraph<T> graph)
        {
            _graph = graph;
            _distance = new Dictionary<Node<T>, int>();
            _predecessor = new Dictionary<Node<T>, Node<T>>();
        }

        public void Run(T startNode)
        {
            Node<T> source = _graph.Nodes.Find(v => v.Data.Equals(startNode));

            if (source == null)
            {
                throw new ArgumentException($"[!] Node {startNode} not found in the graph.");
            }

            factoryDefaults(source);

            List<Node<T>> unvisitedNodes = new List<Node<T>>(_graph.Nodes);

            while (unvisitedNodes.Count > 0)
            {
                _currentNode = minDistance(unvisitedNodes);
                unvisitedNodes.Remove(_currentNode);

                foreach (Edge<T> edge in _currentNode.OutEdges)
                {
                    Node<T> v = edge.To;

                    int weight = (int)edge.Weight;

                    int distanceThroughU = _distance[_currentNode] + weight;

                    if (!_distance.ContainsKey(v) || distanceThroughU < _distance[v])
                    {
                        _distance[v] = distanceThroughU;
                        _predecessor[v] = _currentNode;
                    }
                }
                showStep();
            }
            finalResult();
        }

        private void showStep()
        {
            Console.Clear();
            Console.WriteLine(_graph);
            Console.WriteLine();
            Prompt.Info($"Current Node => ({_currentNode.Data})");
            RenderTable();
            Prompt.Wait();
        }

        private void RenderTable()
        {
            Table table = new Table();

            table.AddColumn("Vertex");
            table.AddColumn("Distance(dv)");
            table.AddColumn("Predecessor(pv)");

            table.Border(TableBorder.Rounded);
            table.Alignment = Justify.Center;
            table.Expand();

            foreach (Node<T> vertex in _graph.Nodes)
            {
                var predecessorData = getPredecessorData(vertex);
                var distanceDisplay = _distance.ContainsKey(vertex) ? _distance[vertex].ToString() : "∞";
                var predecessorDisplay = predecessorData != null ? predecessorData.ToString() : "None";

                if (vertex.Equals(_currentNode))
                {
                    table.AddRow(
                        $"[bold underline yellow on black]{vertex.Data}[/]",
                        $"[bold underline yellow on black]{distanceDisplay}[/]",
                        $"[bold underline yellow on black]{predecessorDisplay}[/]"
                    );
                }
                else
                {
                    table.AddRow(
                        vertex.Data.ToString(),
                        distanceDisplay,
                        predecessorDisplay
                    );
                }
            }

            AnsiConsole.Write(table);
        }

        private Node<T> minDistance(List<Node<T>> unvisitedNodes)
        {
            Node<T> minNode = null;
            int minDistance = int.MaxValue;

            foreach (Node<T> node in unvisitedNodes)
            {
                if (_distance[node] < minDistance)
                {
                    minDistance = _distance[node];
                    minNode = node;
                }
            }
            return minNode;
        }

        private void factoryDefaults(Node<T> source)
        {
            foreach (Node<T> vertex in _graph.Nodes)
            {
                _distance[vertex] = int.MaxValue;
                _predecessor[vertex] = default;
            }

            _distance[source] = 0;
        }

        private T getPredecessorData(Node<T> vertex)
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

        private void finalResult()
        {
            Console.Clear();
            Prompt.Info("\t\t>> Final Result <<");
            RenderTable();
            Prompt.Wait();
        }

    }
}
