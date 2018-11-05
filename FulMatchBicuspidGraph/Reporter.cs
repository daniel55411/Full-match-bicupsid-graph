using System;
using System.Collections.Generic;
using System.IO;
using AdjList = System.Collections.Generic.List<System.Tuple<int, int>>;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace FulMatchBicuspidGraph
{
    internal static class Reporter
    {
        public static void Report(TextWriter writer, bool result, AdjList list)
        {
            if (result)
            {
                writer.WriteLine("Y");
                writer.Write(SuccessReport(list));
            }
            else
            {
                writer.WriteLine("N");
                writer.WriteLine(FailedReport(list));
            }
        }

        private static string SuccessReport(AdjList list)
        {
            var builder = new StringBuilder();
            foreach (var pair in list.OrderBy(tuple => tuple.Item1))
            {
                builder.AppendLine($"{pair.Item1} {pair.Item2}");
            }

            return builder.ToString();
        }

        private static string FailedReport(AdjList list)
        {
            return list.First().Item1.ToString();
        }
    }
}
