using System;
using System.Collections.Generic;
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

            List<Vector2Int> beacons = sensors.Values.ToList();
            beacons.Sort();
            int minX = beacons.First().x;
            int maxX = beacons.Last().x;



            return 0;
        }

        private static bool IsBlocked(Vector2Int cell, Dictionary<Vector2Int, Vector2Int> sensors)
        {
            int distance = int.MaxValue;
            Vector2Int currentSensor = new Vector2Int();

            foreach (Vector2Int sensor in sensors.Keys)
            {
                int tempDist = cell.Manhattan(sensor);
                if (tempDist < distance)
                {
                    distance = tempDist;
                    currentSensor = sensor;
                }
            }

            return distance <= currentSensor.Manhattan(sensors[currentSensor]);
        }
    }
}
