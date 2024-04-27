using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphite.GraphCode
{
    public class Vertex<T>
    {
        public T Data { get; set; }
        public List<Edge<T>> InEdges;
        public List<Edge<T>> OutEdges;

        public Vertex(T value)
        {
            Data = value;
            InEdges = new List<Edge<T>>();
            OutEdges = new List<Edge<T>>();
        }

        public bool AddIncoming(Edge<T> anEdge)
        {
            // Don't add if edge already in list
            if (InEdges.Find(e => e.Equals(anEdge) == true) == null)
            {
                InEdges.Add(anEdge);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddOutgoing(Edge<T> anEdge)
        {
            // Don't add if edge already in list
            if (OutEdges.Find(e => e.Equals(anEdge) == true) == null)
            {
                OutEdges.Add(anEdge);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Data.ToString();
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }
            // If parameter cannot be cast to Point return false.
            Vertex<T> otherNode = obj as Vertex<T>;
            if ((System.Object)otherNode == null)
            {
                return false;
            }
            else
            {
                return this.Equals(otherNode);
            }
        }
        public bool Equals(Vertex<T> otherNode)
        {
            // If parameter is null return false:
            if ((object)otherNode == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Data.Equals(otherNode.Data) == true);
        }
        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

    }


}
