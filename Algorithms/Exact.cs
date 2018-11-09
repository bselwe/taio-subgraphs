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

    public class Exact
    {
        private Set incumbent;
        private Graph G1;
        private Graph G2;
        private ModularGraph G;

        public Exact(Graph G1, Graph G2, ModularGraph G)
        {
            this.G1 = G1;
            this.G2 = G2;
            this.G = G;
        }

        public Set Run()
        {
            incumbent = Sets.Empty();
            Search(Sets.Empty(), Sets.Empty(), G.Vertices);
            return incumbent;
        }

        private void Search(Set solution, Set connected, Set remaining)
        {
            var remainingExceptConnected = remaining.Except(connected).ToHashSet();
            var remainingIntersectConnected = remaining.Intersect(connected).ToHashSet();

            var colorClasses = 
                Coloring.BFS(G, remainingExceptConnected).Concat(
                Coloring.BFS(G, remainingIntersectConnected)).ToList();

            while (colorClasses.Any())
            {
                var lastColorClass = colorClasses.Last(); // O(1) - for lists

                foreach (var v in lastColorClass)
                {
                    if (solution.Count + colorClasses.Count <= incumbent.Count)
                        return;

                    if (!connected.Contains(v) && solution.Any())
                        return;

                    var solutionPrim = solution.Copy();
                    solutionPrim.Add(v);
                    if (solutionPrim.Count > incumbent.Count)
                        incumbent = solutionPrim.Copy();

                    var connectedPrim = connected.Concat(G.Vertices
                        .Where(w => G1.Neighbors[v.Item1].Contains(w.Item1)))
                        .ToHashSet();

                    var remainingPrim = remaining.Intersect(G.Neighbors[v]).ToHashSet();

                    if (remainingPrim.Any())
                        Search(solutionPrim, connectedPrim, remainingPrim);
                }

                colorClasses.Remove(lastColorClass);
            }
        }
    }
}