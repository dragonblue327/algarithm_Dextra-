using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Алгоритм_дейкстры
{
    public class Graph
    {
        public List<Edge> Edges { get; set; }
        public List<Vertex> Vertexes { get; set; }
        public GraphType GraphType { get; set; }

        public Graph()
        {
            Edges = new List<Edge>();
            Vertexes = new List<Vertex>();
            GraphType = GraphType.NonOrientied;
        }
    }

    public enum GraphType
    {
        Orientied,
        NonOrientied
    }
}
