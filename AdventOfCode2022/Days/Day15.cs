using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day15
    {
        public static int BeaconMissing(string input, int line_to_check)
        {
            string[] lines = input.Split("\r\n");
            Dictionary<Vector2Int, Vector2Int> sensors = new Dictionary<Vector2Int, Vector2Int>();
            foreach (string line in lines)
            {
                string[] s = line.Split(' ');
                Vector2Int key = new Vector2Int(Convert.ToInt32(s[2].Substring(2, s[2].Length - 3)), Convert.ToInt32(s[3].Substring(2, s[3].Length - 3)));
                Vector2Int value = new Vector2Int(Convert.ToInt32(s[8].Substring(2, s[8].Length - 3)), Convert.ToInt32(s[9].Substring(2, s[9].Length - 2)));
                sensors.Add(key, value);
            }

            int minX = int.MaxValue;
            int maxX = int.MinValue;
            foreach (var pair in sensors)
            {
                int dist = pair.Key.Manhattan(pair.Value);
                if (pair.Key.x - dist < minX)
                    minX = pair.Key.x - dist;
                if (pair.Key.x + dist > maxX)
                    maxX = pair.Key.x + dist;
            }

            int blockedCells = 0;
            for (int i = minX; i <= maxX; i++)
            {
                if (IsBlocked(new Vector2Int(i, line_to_check), sensors))
                    blockedCells++;
            }

            return blockedCells;
        }

        private static bool IsBlocked(Vector2Int cell, Dictionary<Vector2Int, Vector2Int> sensors)
        {
            if (sensors.Values.Contains(cell))
                return false;

            foreach (var pair in sensors)
            {
                int distance = cell.Manhattan(pair.Key);
                if (distance <= pair.Key.Manhattan(pair.Value))
                    return true;
            }

            return false;
        }
    }
}
