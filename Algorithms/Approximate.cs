using System;
using System.Collections.Generic;
using System.Linq;
using TAiO.Subgraphs.Models;
using TAiO.Subgraphs.Utils;

namespace TAiO.Subgraphs.Algorithms
{
    using Graph = Graph<int>;
    using Vertex = Tuple<int, int>;
    using Set = HashSet<Tuple<int, int>>;

    public class Approximate
    {
        private Set incumbent;
        private Graph G1;
        private Graph G2;
        private ModularGraph G;

        public Approximate(Graph G1, Graph G2, ModularGraph G)
        {
            this.G1 = G1;
            this.G2 = G2;
            this.G = G;
        }

        public Set Run()
        {
            incumbent = Sets.Empty();
            
            foreach (var v in G.Vertices)
            {
                if (d(v) >= incumbent.Count)
                {
                    var solution = new Set() { v };
                    var connected = G.Vertices.Where(w => G1.Neighbors[v.Item1].Contains(w.Item1)).ToHashSet();
                    var remaining = G.Neighbors[v].Where(w => d(w) >= incumbent.Count).ToHashSet();
                    Heuristic(solution, connected, remaining);
                }
            }

            return incumbent;
        }

        private void Heuristic(Set solution, Set connected, Set remaining)
        {
            var U = remaining.Intersect(connected).ToHashSet();

            if (!U.Any())
            {
                if (solution.Count > incumbent.Count)
                    incumbent = solution.Copy();
                return;
            }

            // Find vertex u from U with maximum degree in G
            var u = U.First();
            foreach (var v in U)
                if (d(v) > d(u))
                    u = v;

            var solutionPrim = solution.Copy();
            solutionPrim.Add(u);
            if (solutionPrim.Count > incumbent.Count)
                incumbent = solutionPrim.Copy();

            var connectedPrim = connected.Concat(G.Vertices
                .Where(w => G1.Neighbors[u.Item1].Contains(w.Item1)))
                .ToHashSet();

            var remainingPrim = remaining
                .Intersect(G.Neighbors[u].Where(w => d(w) >= incumbent.Count))
                .ToHashSet();

            Heuristic(solutionPrim, connectedPrim, remainingPrim);
        }

        private int d(Vertex v)
        {
            return G.Neighbors[v].Count;
        }
    }
}