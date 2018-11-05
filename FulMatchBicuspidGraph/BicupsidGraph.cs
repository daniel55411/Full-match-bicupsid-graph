using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FulMatchBicuspidGraph
{
    internal class BicupsidGraph
    {
        public ISet<int> FirstFraction { get; }
        public ISet<int> SecondFraction { get; }
        public IEnumerable<Tuple<int, int>> Edges { get; }
        public int VertexCount => FirstFraction.Count + SecondFraction.Count;

        public BicupsidGraph(ISet<int> firstFraction,
            ISet<int> secondFraction,
            IEnumerable<Tuple<int, int>> edges)
        {
            FirstFraction = firstFraction;
            SecondFraction = secondFraction;
            Edges = edges;
        }
    }
}