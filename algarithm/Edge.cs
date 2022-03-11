using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Алгоритм_дейкстры
{
    public class Edge
    {
        public Vertex StartVertex { get; set; }
        public Vertex EndVertex { get; set; }
        public int Cost { get; set; }
    }
}
