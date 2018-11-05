using System;
using AdjList = System.Collections.Generic.List<System.Tuple<int, int>>;
using System.IO;
using System.Linq;

namespace FulMatchBicuspidGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new StreamReader("in.txt"))
            {
                var graph = Factory.BuildBicupsidGraph(reader);
                var result = Algo.IsFullMatchBicupsidGraph(graph);
                var writer = new StreamWriter("out.txt");
                Reporter.Report(writer, result.Item1, result.Item2);
            }
        }
    }
}