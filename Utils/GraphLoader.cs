using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TAiO.Subgraphs.Models;

namespace TAiO.Subgraphs.Utils
{
    public class GraphLoader
    {
        public static Graph<int> FromCSV(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File '{path}' does not exist.");
            
            var extension = path.Split('.').Last();
            if (extension != "csv")
                throw new NotSupportedException($"Only CSV files are supported.");

            using (var reader = new StreamReader(path))
            {
                var vertices = new List<int>();
                var edges = new List<Tuple<int, int>>();
                var vertex = 0;

                try
                {
                    while (!reader.EndOfStream)
                    {
                        var data = reader.ReadLine();
                        var neighbors = data.Split(',');
                        
                        for (int i = 0; i < neighbors.Length; i++)
                        {
                            var isNeighbor = Int32.Parse(neighbors[i]) == 1;
                            if (isNeighbor)
                                edges.Add(new Tuple<int, int>(vertex, i));
                        }

                        vertices.Add(vertex++);
                    }

                    return new Graph<int>(vertices, edges);
                }
                catch
                {
                    throw new InvalidDataException("Wrong csv file format");
                }
            }
        }
    }
}