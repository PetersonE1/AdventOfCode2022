using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day14
    {
        public static int RestingSand(string input)
        {
            List<Vector2Int> map = GenerateMap(input);
            DisplayMap(map);
            return 0;
        }

        private static List<Vector2Int> GenerateMap(string input)
        {
            List<Vector2Int> map = new List<Vector2Int>();
            string[] lines = input.Split("\r\n");
            foreach (string line in lines)
            {
                Vector2Int[] parts = line.Split(" -> ").Select(v => { int[] xy = v.Split(',').Select(n => Convert.ToInt32(n)).ToArray(); return new Vector2Int(xy[0], xy[1]); }).ToArray();

                map.Add(parts[0]);
                for (int index = 0; index < parts.Length - 1; index++)
                { 
                    while (parts[index] != parts[index + 1])
                    {
                        if (parts[index].x != parts[index + 1].x)
                        {
                            parts[index].x += Math.Clamp(parts[index + 1].x - parts[index].x, -1, 1);
                            map.Add(parts[index]);
                            continue;
                        }
                        if (parts[index].y != parts[index + 1].y)
                        {
                            parts[index].y += Math.Clamp(parts[index + 1].y - parts[index].y, -1, 1);
                            map.Add(parts[index]);
                            continue;
                        }
                    }
                }
            }
            return map;
        }

        private static void DisplayMap(List<Vector2Int> map)
        {
            map.Sort();
            int width = (map.Last().x - map.First().x) + 1;
            int height = map.MaxBy(v => v.y).y + 1;

            Console.WriteLine(map.First());
            Console.WriteLine(map.Last());
            Console.WriteLine($"{width}, {height}");
        }
    }
}
