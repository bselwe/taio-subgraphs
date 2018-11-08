using System;
using TAiO.Subgraphs.Models;

namespace TAiO.Subgraphs.Algorithms
{
    public static class Exact
    {
        public static void Run(Graph<int> G, Graph<int> H, ModularGraph modularGraph)
        {
            var coloring = Coloring.BFS(modularGraph, modularGraph.Vertices);
        }
    }
}