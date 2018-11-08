using System;
using System.Collections.Generic;
using System.Linq;
using TAiO.Subgraphs.Models;

namespace TAiO.Subgraphs.Algorithms
{
    public static class Coloring
    {
        // Greedy BFS coloring
        public static List<HashSet<T>> BFS<T>(Graph<T> G, HashSet<T> verticesToColor)
        {
            var colors = new Dictionary<int, HashSet<T>>();
            var visited = new HashSet<T>();
            var vertexColoring = new Dictionary<T, int>();

            foreach (var start in verticesToColor)
            {
                if (!visited.Contains(start))
                {
                    var queue = new Queue<T>();
                    queue.Enqueue(start);

                    while (queue.Count > 0) 
                    {
                        var vertex = queue.Dequeue();

                        if (visited.Contains(vertex))
                            continue;

                        visited.Add(vertex);
                        
                        var colorUsedByNeighbors = new bool[colors.Count];
                        foreach (var neighbor in G.AdjacencyList[vertex])
                        {
                            if (!verticesToColor.Contains(neighbor))
                                continue;

                            if (!visited.Contains(neighbor))
                                queue.Enqueue(neighbor);

                            if (vertexColoring.ContainsKey(neighbor))
                                colorUsedByNeighbors[vertexColoring[neighbor]] = true;
                        }

                        var color = 0;
                        for (int i = 0; i < colorUsedByNeighbors.Length; i++)
                        {
                            if (!colorUsedByNeighbors[i])
                                break;
                            color = i + 1;                            
                        }

                        if (!colors.ContainsKey(color))
                            colors[color] = new HashSet<T>();
                        colors[color].Add(vertex);
                        vertexColoring[vertex] = color;
                    }
                }
            }

            return colors.Values.ToList();
        }
    }
}