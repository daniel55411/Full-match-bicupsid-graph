﻿using System.Collections.Generic;
using System.Linq;

namespace FulMatchBicuspidGraph
{
    internal class OneThroughputNetwork: INetwork
    {
        public const int ThroughputValue = 1;
        public int VertexCount => _adjancecyList.Length;
        public IEnumerable<int> Vertexes => Enumerable.Range(0, _adjancecyList.Length);

        private readonly int[][] _adjancecyList;

        public OneThroughputNetwork(int[][] adjancecyList)
        {
            //TODO: need copy
            _adjancecyList = adjancecyList;
        }

        public IEnumerable<int> AdjacentVertices(int vertex)
        {
            return _adjancecyList[vertex];
        }

        public int Throughput(int firstVertex, int secondVertex)
        {
            if (firstVertex < _adjancecyList.Length
                && _adjancecyList[firstVertex].Contains(secondVertex))
            {
                return ThroughputValue;
            }

            return 0;
        }
    }
}
