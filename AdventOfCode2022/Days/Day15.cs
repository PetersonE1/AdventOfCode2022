using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day15
    {
        // Adjust to work in ranges
        // width at sensor = manhattan distance
        // width -= 2 each y away
        // at each y, check multiple ranges for continuity
        // if continuity is broken, second range minimum - 1 = beacon
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

        public static int FindBeacon(string input, int search_area)
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

            for (int x = 0; x <= search_area; x++)
            {
                for (int y = 0; y <= search_area; y++)
                {
                    if (!IsBlocked(new Vector2Int(x, y), sensors, false))
                    {
                        Console.WriteLine($"{x}, {y}");
                        return x * 4000000 + y;
                    }
                }
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
}
