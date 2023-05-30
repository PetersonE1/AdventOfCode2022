using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        public static int RestingSand(string input, bool abyss)
        {
            Dictionary<Vector2Int, bool> map = GenerateMap(input);
            int sand = 0;
            int height = map.MaxBy(v => v.Key.y).Key.y;

            bool finished = false;
            while (!finished)
            {
                bool done = false;
                Vector2Int currentPos = new Vector2Int(500, 0);
                while (!done)
                {
                    if (abyss && currentPos.y > height)
                    {
                        finished = true;
                        break;
                    }
                    if (!abyss && currentPos.y == height + 1)
                    {
                        map.Add(currentPos, true);
                        sand++;
                        break;
                    }
                    if (!map.ContainsKey(currentPos + new Vector2Int(0, 1)))
                    {
                        currentPos.y++;
                        continue;
                    }
                    if (!map.ContainsKey(currentPos + new Vector2Int(-1, 1)))
                    {
                        currentPos.y++;
                        currentPos.x--;
                        continue;
                    }
                    if (!map.ContainsKey(currentPos + new Vector2Int(1, 1)))
                    {
                        currentPos.y++;
                        currentPos.x++;
                        continue;
                    }
                    if (currentPos == new Vector2Int(500, 0))
                    {
                        sand++;
                        finished = true;
                        break;
                    }
                    map.Add(currentPos, true);
                    sand++;
                    done = true;
                }
            }

            DisplayMap(map);
            return sand;
        }

        private static Dictionary<Vector2Int, bool> GenerateMap(string input)
        {
            Dictionary<Vector2Int, bool> map = new Dictionary<Vector2Int, bool>();
            string[] lines = input.Split("\r\n");
            foreach (string line in lines)
            {
                Vector2Int[] parts = line.Split(" -> ").Select(v => { int[] xy = v.Split(',').Select(n => Convert.ToInt32(n)).ToArray(); return new Vector2Int(xy[0], xy[1]); }).ToArray();

                if (!map.ContainsKey(parts[0]))
                    map.Add(parts[0], false);
                for (int index = 0; index < parts.Length - 1; index++)
                { 
                    while (parts[index] != parts[index + 1])
                    {
                        if (parts[index].x != parts[index + 1].x)
                        {
                            parts[index].x += Math.Clamp(parts[index + 1].x - parts[index].x, -1, 1);
                            if (!map.ContainsKey(parts[index]))
                                map.Add(parts[index], false);
                            continue;
                        }
                        if (parts[index].y != parts[index + 1].y)
                        {
                            parts[index].y += Math.Clamp(parts[index + 1].y - parts[index].y, -1, 1);
                            if (!map.ContainsKey(parts[index]))
                                map.Add(parts[index], false);
                            continue;
                        }
                    }
                }
            }
            return map;
        }

        private static void DisplayMap(Dictionary<Vector2Int, bool> mapInput)
        {
            ImmutableSortedDictionary<Vector2Int, bool> map = mapInput.ToImmutableSortedDictionary();
            int minX = map.First().Key.x;
            int width = (map.Last().Key.x - map.First().Key.x) + 1;
            int height = map.MaxBy(v => v.Key.y).Key.y + 1;

            Console.WriteLine(map.First().Key);
            Console.WriteLine(map.Last().Key);
            Console.WriteLine($"{width}, {height}\n");

            for (int y = 0; y < height; y++)
            {
                string s = string.Empty;
                for (int x = 0; x < width; x++)
                {
                    if (x + minX == 500 && y == 0)
                    {
                        s += '+';
                        continue;
                    }
                    Vector2Int v = new Vector2Int(x + minX, y);
                    if (map.ContainsKey(v))
                    {
                        if (!map[v])
                            s += '#';
                        else
                            s += 'O';
                    }
                    else
                        s += '.';
                }
                Console.WriteLine(s);
            }
        }
    }
}
