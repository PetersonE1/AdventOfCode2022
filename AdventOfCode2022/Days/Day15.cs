using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day15
    {
        public static int BeaconMissing(string input, int line_to_check)
        {
            string[] lines = input.Split("\r\n");
            Dictionary<Vector2Int, Beacon> sensors = new Dictionary<Vector2Int, Beacon>();
            foreach (string line in lines)
            {
                string[] s = line.Split(' ');
                Vector2Int key = new Vector2Int(Convert.ToInt32(s[2].Substring(2, s[2].Length - 3)), Convert.ToInt32(s[3].Substring(2, s[3].Length - 3)));
                Vector2Int value = new Vector2Int(Convert.ToInt32(s[8].Substring(2, s[8].Length - 3)), Convert.ToInt32(s[9].Substring(2, s[9].Length - 2)));
                sensors.Add(key, new Beacon(value, key.Manhattan(value)));
            }

            int minX = int.MaxValue;
            int maxX = int.MinValue;
            foreach (var pair in sensors)
            {
                int dist = pair.Value.distance;
                if (pair.Key.x - dist < minX)
                    minX = pair.Key.x - dist;
                if (pair.Key.x + dist > maxX)
                    maxX = pair.Key.x + dist;
            }

            int blockedCells = 0;
            for (int i = minX; i <= maxX; i++)
            {
                if (IsBlocked(new Vector2Int(i, line_to_check), sensors, true))
                    blockedCells++;
            }

            return blockedCells;
        }

        public static ulong FindBeacon(string input, int search_area)
        {
            string[] lines = input.Split("\r\n");
            Dictionary<Vector2Int, Beacon> sensors = new Dictionary<Vector2Int, Beacon>();
            foreach (string line in lines)
            {
                string[] s = line.Split(' ');
                Vector2Int key = new Vector2Int(Convert.ToInt32(s[2].Substring(2, s[2].Length - 3)), Convert.ToInt32(s[3].Substring(2, s[3].Length - 3)));
                Vector2Int value = new Vector2Int(Convert.ToInt32(s[8].Substring(2, s[8].Length - 3)), Convert.ToInt32(s[9].Substring(2, s[9].Length - 2)));
                sensors.Add(key, new Beacon(value, key.Manhattan(value)));
            }

            List<Tuple<int, Tuple<int, int>>> ranges = new List<Tuple<int, Tuple<int, int>>>();
            foreach (var pair in sensors)
            {
                int y = pair.Key.y;
                int x = pair.Key.x;
                for (int offset = 0; offset < pair.Value.distance; offset++)
                {
                    int yMin = y - offset;
                    int yMax = y + offset;
                    int xOffset = pair.Value.distance - offset;
                    int xMin = x - xOffset;
                    int xMax = x + xOffset;
                    if (xMin < 0)
                        xMin = 0;
                    if (xMax > search_area)
                        xMax = search_area;
                    Tuple<int, int> r = new Tuple<int, int>(xMin, xMax);
                    if (yMin >= 0)
                        ranges.Add(new Tuple<int, Tuple<int, int>>(y - offset, r));
                    if (yMax <= search_area)
                        ranges.Add(new Tuple<int, Tuple<int, int>>(y + offset, r));
                }
            }

            ranges = ranges.Distinct().ToList();
            ranges.Sort(new TupleIntKeySort());

            int cachedY = -1;
            Tuple<int, int> cachedRange = new Tuple<int, int>(0, 0);
            foreach (Tuple<int, Tuple<int, int>> range in ranges)
            {
                if (cachedY != range.Item1)
                {
                    cachedY = range.Item1;
                    cachedRange = range.Item2;
                    continue;
                }
                if ((range.Item2.Item1 > cachedRange.Item2 + 1 && range.Item2.Item2 > cachedRange.Item2 + 1) || (range.Item2.Item1 + 1 < cachedRange.Item1 && range.Item2.Item2 + 1 < cachedRange.Item1))
                {
                    return (ulong)(range.Item2.Item1 - 1) * 4000000 + (uint)range.Item1;
                }
                if (range.Item2.Item1 >= cachedRange.Item1 && range.Item2.Item2 <= cachedRange.Item2)
                    continue;
                cachedRange = range.Item2;
            }

            return 0;
        }

        private static bool IsBlocked(Vector2Int cell, Dictionary<Vector2Int, Beacon> sensors, bool checkExisting)
        {
            if (checkExisting && sensors.Values.Contains(new Beacon(cell, 0), new BeaconComparePos()))
                return false;

            foreach (var pair in sensors)
            {
                int distance = cell.Manhattan(pair.Key);
                if (distance <= pair.Value.distance)
                    return true;
            }

            return false;
        }
    }

    internal class Beacon
    {
        public Vector2Int pos;
        public int distance;

        public Beacon(Vector2Int pos, int distance)
        {
            this.pos = pos;
            this.distance = distance;
        }
    }

    internal class BeaconComparePos : IEqualityComparer<Beacon>
    {
        public bool Equals(Beacon? x, Beacon? y)
        {
            return x.pos == y.pos;
        }

        public int GetHashCode([DisallowNull] Beacon obj)
        {
            return obj.pos.GetHashCode();
        }
    }

    internal class TupleIntKeySort : IComparer<Tuple<int, Tuple<int, int>>>
    {
        public int Compare(Tuple<int, Tuple<int, int>>? x, Tuple<int, Tuple<int, int>>? y)
        {
            int result = x.Item1 - y.Item1;
            if (result == 0)
            {
                return x.Item2.Item1 - y.Item2.Item1;
            }
            return result;
        }
    }
}
