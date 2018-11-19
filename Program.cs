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
            bool isGraphsChecker = IsValidGraphsChecker(args);
            bool isComplexityChecker = IsValidComplexityChecker(args);

            if (!(isGraphsChecker ^ isComplexityChecker))
            {
                ShowHelp(args);
                return;
            }

            bool exact = args[0] == "exact";

            if (isGraphsChecker)
            {
                var G = GraphLoader.FromCSV(args[1]);
                var H = GraphLoader.FromCSV(args[2]);
                var modularGraph = ModularGraph.Create(G, H);

                if (exact) 
                {
                    var exactResult = new Exact(G, H, modularGraph).Run();
                    GraphLoader.ToCSV(exactResult, "Examples/exact.csv");
                }
                else
                {
                    var approximateResult = new Approximate(G, H, modularGraph).Run();
                    GraphLoader.ToCSV(approximateResult, "Examples/approximate.csv");
                }
            }
            else
            {
                new ComplexityChecker(args[2], exact).Run();
            }
        }

        static bool IsValidGraphsChecker(string[] args)
        {
            return AreValidArgs(args) && args[1] != "complexity";
        }

        static bool IsValidComplexityChecker(string[] args)
        {
            return AreValidArgs(args) && args[1] == "complexity";
        }

        static bool AreValidArgs(string[] args)
        {
            return args.Length == 3 && (args[0] == "exact" || args[0] == "approximate");
        }

        static void ShowHelp(string[] args)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("dotnet run (exact|approximate) (path-to-first-graph) (path-to-second-graph)");
            Console.WriteLine("dotnet run (exact|approximate) complexity (examples-name)");
            Console.WriteLine("Graps are represented via CSV files.");
        }
    }
}
