using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TAiO.Subgraphs.Models;

namespace TAiO.Subgraphs.Utils
{
    using Graph = Graph<int>;
    using Edge = Tuple<int, int>;
    using Set = HashSet<Tuple<int, int>>;

    public class GraphLoader
    {
        public static Graph FromCSV(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File '{path}' does not exist.");
            
            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension) || extension != ".csv")
                throw new NotSupportedException("Only CSV files are supported.");

            using (var reader = new StreamReader(path))
            {
                try
                {
                    var vertices = new List<int>();
                    var edges = new List<Edge>();
                    var vertex = 0;

                    while (!reader.EndOfStream)
                    {
                        var data = reader.ReadLine();
                        var neighbors = data.Split(',');
                        
                        for (int i = 0; i < neighbors.Length; i++)
                        {
                            var isNeighbor = Int32.Parse(neighbors[i]) == 1;
                            if (isNeighbor)
                                edges.Add(new Edge(vertex, i));
                        }

                        vertices.Add(vertex++);
                    }

                    return new Graph(vertices, edges);
                }
                catch
                {
                    throw new FormatException("Wrong csv file format");
                }
            }
        }

        public static void ToCSV(Set matching, string path)
        {
            var file = File.Create(path);

            using (var writer = new StreamWriter(file))
            {
                var firstSubgraph = string.Join(',', matching.Select(v => v.Item1));
                var secondSubgraph = string.Join(',', matching.Select(v => v.Item2));
                
                writer.WriteLine(firstSubgraph);
                writer.WriteLine(secondSubgraph);
            }

            file.Close();
        }
    }
}