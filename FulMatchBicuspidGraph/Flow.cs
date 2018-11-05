using System;
using System.Collections.Generic;
using System.Linq;

namespace FulMatchBicuspidGraph
{
    internal class Flow
    {
        public INetwork OneThroughputNetwork { get; }
        public int Source { get; }
        public int Sink { get; }
        public int Value => _flowValues
            .Where(pair => pair.Key.Item2 == Sink)
            .Sum(pair => pair.Value);

        public IEnumerable<Tuple<int, int>> Edges => _flowValues
            .Where(pair => pair.Value != 0)
            .Select(pair => pair.Key);

        private readonly IDictionary<Tuple<int, int>, int> _flowValues;

        public Flow(INetwork network, int source, int sink)
        {
            OneThroughputNetwork = network;
            Source = source;
            Sink = sink;
            _flowValues = new Dictionary<Tuple<int, int>, int>();
            FillFlow(network, 0);
        }

        public int Get(Tuple<int, int> tuple)
        {
            return _flowValues.ContainsKey(tuple)
                ? _flowValues[tuple]
                : throw new Exception($"Edge({tuple.Item1} -> {tuple.Item2}) not found");
        }

        public int Get(int firstVertex, int secondVertex)
        {
            var tuple = Tuple.Create(firstVertex, secondVertex);
            return Get(tuple);
        }

        public bool TryUpdate(int firstVertex, int secondVertex, int value)
        {
            var tuple = Tuple.Create(firstVertex, secondVertex);
            if (!_flowValues.ContainsKey(tuple))
            {
                return false;
            }

            _flowValues[tuple] += value;
            return true;
        }

        private void FillFlow(INetwork network, int flow)
        {
            foreach (var firstVertex in network.Vertexes)
            {
                foreach (var secondVertex in network.AdjacentVertices(firstVertex))
                {
                    _flowValues.Add(Tuple.Create(firstVertex, secondVertex), flow);
                }
            }
        }
    }
}
