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

        public int Value => GetEdges()
            .Where(pair => pair.Item2 == Sink)
            .Select(tuple => _flowValues[tuple.Item1, tuple.Item2])
            .Sum();

        public IEnumerable<Tuple<int, int>> Edges => GetEdges();

        private readonly int[,] _flowValues;

        public Flow(INetwork network, int source, int sink)
        {
            OneThroughputNetwork = network;
            Source = source;
            Sink = sink;
            _flowValues = new int[network.VertexCount, network.VertexCount];
            FillFlow();
        }

        public int Get(int firstVertex, int secondVertex)
        {
            return _flowValues[firstVertex, secondVertex];
        }

        public void Update(int firstVertex, int secondVertex, int value)
        {
            _flowValues[firstVertex, secondVertex] += value;
        }

        private void FillFlow()
        {
            for (var i = 0; i < _flowValues.GetLength(0); i++)
            {
                for (var j = 0; j < _flowValues.GetLength(0); j++)
                {
                    _flowValues[i, j] = 0;
                }
            }
        }

        private IEnumerable<Tuple<int, int>> GetEdges()
        {
            for (var i = 0; i < _flowValues.GetLength(0); i++)
            {
                for (var j = 0; j < _flowValues.GetLength(0); j++)
                {
                    if (_flowValues[i, j] > 0)
                    {
                        yield return Tuple.Create(i, j);
                    }
                }
            }
        }
    }
}
