using System.Collections.Generic;

namespace FulMatchBicuspidGraph
{
    internal interface INetwork
    {
        int VertexCount { get; }
        IEnumerable<int> Vertexes { get; }
        IEnumerable<int> AdjacentVertices(int vertex);
        int Throughput(int firstVertex, int secondVertex);
    }
}
