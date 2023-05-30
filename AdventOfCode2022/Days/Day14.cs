using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day14
    {
        public static int RestingSand(string input)
        {
            return 0;
        }

        private static List<Vector2Int> GenerateMap(string input)
        {
            List<Vector2Int> map = new List<Vector2Int>();
            string[] lines = input.Split("\r\n");
            foreach (string line in lines)
            {
                Vector2Int[] parts = line.Split(" -> ").Select(v => { int[] xy = v.Split(',').Select(n => Convert.ToInt32(n)).ToArray(); return new Vector2Int(xy[0], xy[1]); }).ToArray();
                int index = 0;

                map.Add(parts[0]);
                while (index < parts.Length - 1 && parts[index] != parts[index + 1])
                {
                    Vector2Int first = parts[index];
                    Vector2Int second = parts[index + 1];
                    if (first.x != second.x)
                    {
                        first.x += Math.Clamp(second.x - first.x, -1, 1);
                        map.Add(first);
                        continue;
                    }
                    if (first.y != second.y)
                    {
                        first.y += Math.Clamp(second.y - first.y, -1, 1);
                        map.Add(first);
                        continue;
                    }
                    index++;
                }
            }
            return map;
        }

        private static void DisplayMap(List<Vector2Int> map)
        {
            map.Sort();

        }
    }
}
