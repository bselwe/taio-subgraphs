using System;
using System.Collections.Generic;

namespace TAiO.Subgraphs.Models
{
    using Vertex = Tuple<int, int>;
    
    public class ModularGraph : Graph<Vertex>
    {
        public ModularGraph(IEnumerable<Vertex> vertices, IEnumerable<Tuple<Vertex, Vertex>> edges) 
            : base(vertices, edges)
        {
        }

        public static ModularGraph Create(Graph<int> G, Graph<int> H)
        {
            var vertices = new List<Vertex>();
            var edges = new List<Tuple<Vertex, Vertex>>();

            for (int i = 0; i < G.Neighbors.Count; i++)
                for (int j = 0; j < H.Neighbors.Count; j++)
                    vertices.Add(new Vertex(i, j));

            for (int i = 0; i < vertices.Count; i++)
            {
                var firstVertex = vertices[i];

                for (int j = 0; j < vertices.Count; j++)
                {
                    if (i == j) 
                        continue;

                    var secondVertex = vertices[j];

                    var u = firstVertex.Item1;
                    var v = firstVertex.Item2;
                    var uPrim = secondVertex.Item1;
                    var vPrim = secondVertex.Item2;

                    if (u == uPrim || v == vPrim)
                        continue;

                    var adjacentCondition = 
                        G.Neighbors[u].Contains(uPrim) && 
                        H.Neighbors[v].Contains(vPrim);

                    var nonAdjacentCondition =
                        !G.Neighbors[u].Contains(uPrim) &&
                        !H.Neighbors[v].Contains(vPrim);

                    if (adjacentCondition || nonAdjacentCondition)
                        edges.Add(new Tuple<Vertex, Vertex>(firstVertex, secondVertex)); 
                }
            }

            return new ModularGraph(vertices, edges);
        }
    }
}