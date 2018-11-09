using System;
using TAiO.Subgraphs.Algorithms;
using TAiO.Subgraphs.Models;
using TAiO.Subgraphs.Utils;

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

            var G = GraphLoader.FromCSV(args[0]);
            var H = GraphLoader.FromCSV(args[1]);
            var modularGraph = ModularGraph.Create(G, H);

            var exactResult = new Exact(G, H, modularGraph).Run();
            var approximateResult = new Approximate(G, H, modularGraph).Run();

            GraphLoader.ToCSV(exactResult, "Data/exact.csv");
            GraphLoader.ToCSV(approximateResult, "Data/approximate.csv");
        }
    }
}
