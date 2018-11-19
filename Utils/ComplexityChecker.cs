using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TAiO.Subgraphs.Algorithms;
using TAiO.Subgraphs.Models;

namespace TAiO.Subgraphs.Utils
{
    using Graph = Graph<int>;
    using Edge = Tuple<int, int>;
    using Set = HashSet<Tuple<int, int>>;

    public class ComplexityChecker
    {
        private readonly string complexityDirectory = "Complexity";
        private readonly string complexityFile = "Complexity/complexity.csv";

        private readonly string examplesDirectory; 
        private readonly bool exact;
        private int examplesCount;

        private StreamWriter outFile;

        public ComplexityChecker(string examplesName, bool exact)
        {
            this.examplesDirectory = $"{complexityDirectory}/{examplesName}"; 
            this.exact = exact;
        }

        public void Run()
        {
            PrepareDirectories();

            var directories = Directory.GetDirectories(examplesDirectory);
            examplesCount = directories.Length;

            for (int i = 0; i < directories.Length; i++)
            {
                var directory = directories[i];
                var files = Directory.GetFiles(directory);
                
                if (files.Length != 2) 
                    throw new InvalidDataException($"Invalid number of files in directory '{directory}', expected: 2.");
                
                var pathG = files.FirstOrDefault(f => f.Contains("_A_"));
                var pathH = files.FirstOrDefault(f => f.Contains("_B_"));

                if (!(pathG != null && pathH != null))
                    throw new InvalidDataException($"Could not find graphs A and B in directory '{directory}'.");

                RunAlgorithm(pathG, pathH, i);
            }

            Dispose();
        }

        private void RunAlgorithm(string pathG, string pathH, int step)
        {
            var G = GraphLoader.FromCSV(pathG);
            var H = GraphLoader.FromCSV(pathH);
            var modularGraph = ModularGraph.Create(G, H);
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (this.exact)
                new Exact(G, H, modularGraph).Run();
            else
                new Approximate(G, H, modularGraph).Run();

            watch.Stop();
            WriteResult(G, H, watch.ElapsedMilliseconds, step);
        }

        private void WriteResult(Graph G, Graph H, long ms, int step)
        {
            outFile.WriteLine($"{G.Vertices.Count},{H.Vertices.Count},{ms}");
            Console.WriteLine($"[{step + 1}/{examplesCount}] Result for G (|V| = {G.Vertices.Count}), H (|V| = {H.Vertices.Count}): {ms} ms");
        }

        private void PrepareDirectories()
        {
            if (!Directory.Exists(complexityDirectory))
                Directory.CreateDirectory(complexityDirectory);

            if (!Directory.Exists(examplesDirectory))
                throw new DirectoryNotFoundException($"File '{examplesDirectory}' does not exist.");

            outFile = File.CreateText(complexityFile);
        }

        private void Dispose()
        {
            outFile.Dispose();
        }
    }
}