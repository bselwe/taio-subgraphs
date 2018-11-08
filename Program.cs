using System;

namespace TAiO.Subgraphs
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("dotnet run (path-to-first-graph) (path-to-second-graph)");
                Console.WriteLine("Graps are represented via CSV files.");
                return;
            }

            var firstGraph = GraphLoader.FromCSV(args[0]);
            var secondGraph = GraphLoader.FromCSV(args[1]);
        }
    }
}
