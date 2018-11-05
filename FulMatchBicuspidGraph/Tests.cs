using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestFlow = System.Collections.Generic.Dictionary<System.Tuple<int, int>, int>;
using NUnit.Framework;

namespace FulMatchBicuspidGraph
{
    [TestFixture]
    internal class Tests
    {
        private const string Test1 = @"2 3
1 1 0
1 0 1";

        private const string Test2 = @"3 3
1 1 0
1 0 1
0 1 1";

        private const string Test3 = @"3 3
1 1 1
1 0 0
1 0 1";

        private const string Test4 = @"3 3
1 1 1
1 0 0
1 0 0";

        [Test]
        public void FactoryTest_BuildBicupsidGraph()
        {
            var expected = new BicupsidGraph(
                new HashSet<int>(new[] {1, 2}),
                new HashSet<int>(new[] {1, 2, 3}),
                new[]
                {
                    Tuple.Create(1, 1),
                    Tuple.Create(1, 2),
                    Tuple.Create(2, 1),
                    Tuple.Create(2, 3),
                }
            );
            var actual = Factory.BuildBicupsidGraph(new StringReader(Test1));
            Assert.IsTrue(Equals(expected, actual));
        }

        [Test]
        public void FactoryTest_BuildNetwork()
        {
            var expected = new OneThroughputNetwork(
                new[]
                {
                    new[] {1, 2},
                    new[] {3, 4 },
                    new[] {3, 5},
                    new[] {6},
                    new[] {6},
                    new[] {6},
                    new int[] {}, 
                }
            );
            var graph = Factory.BuildBicupsidGraph(new StringReader(Test1));
            var actual = Factory.BuildOneThroughputNetwork(graph);

            Assert.IsTrue(Equals(expected, actual));
        }

        [Test]
        public void AlgoTest_MaxFlow()
        {
            var graph = Factory.BuildBicupsidGraph(new StringReader(Test1));
            var network = Factory.BuildOneThroughputNetwork(graph);
            var flow = Algo.MaxFlow(network);
            var expectedEdges = new TestFlow
            {
                {Tuple.Create(0, 1), 1},
                {Tuple.Create(0, 2), 1},
                {Tuple.Create(1, 3), 1},
                {Tuple.Create(2, 5), 1},
                {Tuple.Create(3, 6), 1},
                {Tuple.Create(5, 6), 1},
            };

            Assert.AreEqual(2, flow.Value);
            Assert.IsTrue(Equals(expectedEdges, flow));
        }

        [Test]
        public void AlgoTest_IsFullMatchBicupsidGraph_True_1()
        {
            var graph = Factory.BuildBicupsidGraph(new StringReader(Test2));
            var result = Algo.IsFullMatchBicupsidGraph(graph);
            var expected = new[]
            {
                Tuple.Create(1, 1),
                Tuple.Create(2, 3),
                Tuple.Create(3, 2),
            };

            Assert.IsTrue(result.Item1, "Is full match");
            Assert.IsTrue(result.Item2.SequenceEqual(expected));
        }

        [Test]
        public void AlgoTest_IsFullMatchBicupsidGraph_True_2()
        {
            var graph = Factory.BuildBicupsidGraph(new StringReader(Test3));
            var result = Algo.IsFullMatchBicupsidGraph(graph);
            var expected = new[]
            {
                Tuple.Create(1, 2),
                Tuple.Create(2, 1),
                Tuple.Create(3, 3),
            };

            Assert.IsTrue(result.Item1, "Is full match");
            Assert.IsTrue(result.Item2.SequenceEqual(expected));
        }

        [Test]
        public void AlgoTest_IsFullMatchBicupsidGraph_False_NotEqualFractions()
        {
            var graph = Factory.BuildBicupsidGraph(new StringReader(Test1));
            var result = Algo.IsFullMatchBicupsidGraph(graph);

            Assert.IsFalse(result.Item1);
            Assert.AreEqual(1, result.Item2.First().Item1);
        }

        [Test]
        public void AlgoTest_IsFullMatchBicupsidGraph_False_NotFullMatch()
        {
            var graph = Factory.BuildBicupsidGraph(new StringReader(Test4));
            var result = Algo.IsFullMatchBicupsidGraph(graph);

            Assert.IsFalse(result.Item1);
            Assert.AreEqual(2, result.Item2.First().Item1);
        }

        private bool Equals(BicupsidGraph first, BicupsidGraph second)
        {
            return first.FirstFraction.SequenceEqual(second.FirstFraction)
                   && first.SecondFraction.SequenceEqual(second.SecondFraction)
                   && first.Edges.SequenceEqual(second.Edges);
        }

        private bool Equals(INetwork first, INetwork second)
        {
            if (first.VertexCount != second.VertexCount)
            {
                return false;
            }

            return first.Vertexes
                .All(vertex => first.AdjacentVertices(vertex).SequenceEqual(second.AdjacentVertices(vertex)));
        }

        private bool Equals(TestFlow expected, Flow actual)
        {
            if (!expected.Keys.SequenceEqual(actual.Edges))
            {
                return false;
            }

            foreach (var tuple in actual.Edges)
            {
                if (actual.Get(tuple) != expected[tuple])
                {
                    return false;
                }
            }

            return true;

        }
    }
}