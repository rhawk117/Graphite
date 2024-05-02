using Graphite.GraphCode;
using Graphite.Menu;
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
            // to anyone who is using this repo, project isn't polished yet 
            // and likely has multiple bugs heads up

            // also dowells graph class is really slow for some reason

            Prompt.Info("\tWelcome to Graphite, loading program...");
            Utils.DisplayHeader();
            GraphUI graphUI = new GraphUI();
            try
            {
                graphUI.Run();
            }
            catch (Exception ex)
            {
                Prompt.ErrorHandler(ex);
            }
        }



        static void dijkstrasTest()
        {
            OurGraph<int> graph = new OurGraph<int>(true);
            graph.AddNode(1);
            graph.AddNode(2);
            graph.AddNode(3);
            graph.AddNode(4);
            graph.AddNode(5);
            graph.AddNode(6);
            graph.AddNode(7);

            // src, dst, weight
            // 1
            graph.AddEdge(1, 2, 2);
            graph.AddEdge(1, 1, 4);
            // 2
            graph.AddEdge(2, 4, 3);
            graph.AddEdge(2, 5, 10);

            // 3
            graph.AddEdge(3, 1, 4);

            // 4
            graph.AddEdge(4, 3, 2);
            graph.AddEdge(4, 5, 2);
            graph.AddEdge(4, 6, 8);
            graph.AddEdge(4, 7, 4);
            // 5
            graph.AddEdge(5, 7, 6);

            // 6 

            // 7

            graph.AddEdge(7, 6, 1);


            //graph.DirectedView();


            Dijkstra<int> dijkstra = new Dijkstra<int>(graph);
            dijkstra.Run(1);

        }
    }
}
