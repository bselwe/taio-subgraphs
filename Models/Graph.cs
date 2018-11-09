using System;
using System.Collections.Generic;
using System.IO;

namespace TAiO.Subgraphs.Models
{
    public class Graph<T>
    {
        public Dictionary<T, HashSet<T>> Neighbors { get; } = new Dictionary<T, HashSet<T>>();
        public HashSet<T> Vertices { get; private set; }

        public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges) 
        {
            Vertices = new HashSet<T>(vertices); 

            foreach(var vertex in vertices)
                AddVertex(vertex);

            foreach(var edge in edges)
                AddEdge(edge);
        }

        public void AddVertex(T vertex) 
        {
            Neighbors[vertex] = new HashSet<T>();
        }

        public void AddEdge(Tuple<T, T> edge) 
        {
            if (Neighbors.ContainsKey(edge.Item1) && Neighbors.ContainsKey(edge.Item2)) 
            {
                Neighbors[edge.Item1].Add(edge.Item2);
                Neighbors[edge.Item2].Add(edge.Item1);
            }
        }
    }
}