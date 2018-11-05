using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FulMatchBicuspidGraph
{
    internal static class Factory
    {
        public static BicupsidGraph BuildBicupsidGraph(TextReader reader)
        {
            var sizes = reader
                .ReadLine()
                ?.Split(' ')
                .Select(int.Parse)
                .ToArray();
            Debug.Assert(sizes != null, nameof(sizes) + " != null");

            var firstHalf = new HashSet<int>(Enumerable.Range(1, sizes[0]));
            var secondHalf = new HashSet<int>(Enumerable.Range(1, sizes[1]));
            var edges = new List<Tuple<int, int>>();

            for (var i = 1; i < sizes[0] + 1; i++)
            {
                var values = reader
                    .ReadLine()
                    ?.Split(' ')
                    .Select(int.Parse)
                    .ToArray();

                Debug.Assert(values != null, nameof(values) + " != null");

                for (var j = 1; j < sizes[1] + 1; j++)
                {
                    if (values[j - 1] == 1)
                    {
                        edges.Add(Tuple.Create(i, j));
                    }
                }
            }

            return new BicupsidGraph(firstHalf, secondHalf, edges);
        }

        public static INetwork BuildOneThroughputNetwork(BicupsidGraph graph)
        {
            const int source = 0;
            var sink = graph.VertexCount + 1;
            var adjancecyList = graph.Edges
                .OrderBy(tuple => tuple.Item1)
                .GroupBy(tuple => tuple.Item1)
                .Select(tuples => tuples
                    .Select(tuple => tuple.Item2 + graph.FirstFraction.Count)
                    .ToArray())
                .ToList();

            adjancecyList.Insert(source, graph.FirstFraction
                .OrderBy(i => i)
                .ToArray());

            adjancecyList.AddRange(graph.SecondFraction.Select(vertex => new[] {sink}));
            adjancecyList.Add(new int[]{});

            return new OneThroughputNetwork(adjancecyList.ToArray());
        }
    }
}
