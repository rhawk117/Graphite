using Graphite.GraphCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OurGraph<int> graph = new OurGraph<int>(true);
            graph.AddNode(1);
            graph.AddNode(2);
            graph.AddNode(3);
            graph.AddNode(4);
            graph.AddNode(5);


            graph.AddEdge(1, 2, 10);
            graph.AddEdge(1, 3, 5);
            graph.AddEdge(2, 4, 1);
            graph.AddEdge(3, 4, 2);
            graph.AddEdge(3, 5, 8);
            graph.AddEdge(4, 5, 4);


            graph.DirectedView();


            Dijkstra<int> dijkstra = new Dijkstra<int>(graph);
            dijkstra.Run(1);



        }
    }
}
