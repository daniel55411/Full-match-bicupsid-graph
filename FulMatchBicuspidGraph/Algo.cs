using System;
using AdjList = System.Collections.Generic.List<System.Tuple<int, int>>;
using System.Collections.Generic;
using System.Linq;

namespace FulMatchBicuspidGraph
{
    internal static class Algo
    {
        public static Tuple<bool, AdjList> IsFullMatchBicupsidGraph(BicupsidGraph graph)
        {
            AdjList result;

            if (graph.FirstFraction.Count != graph.SecondFraction.Count)
            {
                result = new AdjList(new[]
                {
                    Tuple.Create(graph.Edges.Min(tuple => tuple.Item1), 0)
                });
                return Tuple.Create(false, result);
            }

            var network = Factory.BuildOneThroughputNetwork(graph);
            var maxFlow = MaxFlow(network);

            if (graph.FirstFraction.Count == maxFlow.Value)
            {
                result = graph.Edges
                    .Where(tuple => 
                        maxFlow.Edges.Contains(Tuple.Create(tuple.Item1, tuple.Item2 + graph.FirstFraction.Count)))
                    .ToList();
                return Tuple.Create(true, result);
            }

            result = new AdjList();
            var item = graph.Edges
                .GroupBy(tuple => tuple.Item1)
                .First(tuples => tuples.All(
                    tuple => maxFlow.Get(Tuple.Create(tuple.Item1, tuple.Item2 + graph.FirstFraction.Count)) == 0)
                ).Key;
            result.Add(Tuple.Create(item, 0));

            return Tuple.Create(false, result);
        }

        public static Flow MaxFlow(INetwork network)
        {
            var visited = Enumerable
                .Range(0, network.VertexCount)
                .Select(i => false)
                .ToArray();
            var flow = new Flow(network, 0, network.VertexCount - 1);

            while (FindPath(flow.Source, network, visited, flow, int.MaxValue) != 0)
            {
                /* EMPTY */
            }

            return flow;
        }

        private static int FindPath(int currentVertex,
            INetwork network,
            IList<bool> visited,
            Flow flow,
            int minFlow)
        {
            if (currentVertex == flow.Sink)
            {
                return minFlow;
            }

            visited[currentVertex] = true;
            foreach (var vertex in network.AdjacentVertices(currentVertex))
            {
                var flowValue = flow.Get(currentVertex, vertex);
                var throughput = network.Throughput(currentVertex, vertex);

                if (!visited[vertex] && flowValue < throughput)
                {
                    var delta = FindPath(vertex, 
                        network, visited, flow, 
                        Math.Min(minFlow, throughput - flowValue));
                    if (delta > 0)
                    {
                        flow.TryUpdate(currentVertex, vertex, delta);
                        flow.TryUpdate(vertex, currentVertex, -delta);
                        return delta;
                    }
                }
            }

            return 0;
        }
    }
}
