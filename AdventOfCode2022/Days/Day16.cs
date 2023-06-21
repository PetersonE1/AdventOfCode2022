using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day16
    {
        const int INF = 99999;
        private static int CurrentBest;
        private static int[] FinalPath;

        private static void Print(int[,] distance, int verticesCount)
        {
            for (int i = 0; i < verticesCount; ++i)
            {
                for (int j = 0; j < verticesCount; ++j)
                {
                    if (distance[i, j] == INF)
                        Console.Write("INF".PadLeft(7));
                    else
                        Console.Write(distance[i, j].ToString().PadLeft(7));
                }

                Console.WriteLine();
            }
        }

        public static int[,] FloydWarshall(int[,] graph, int verticesCount, out int[,] prev)
        {
            int[,] distance = new int[verticesCount, verticesCount];
            prev = new int[verticesCount, verticesCount];

            for (int i = 0; i < verticesCount; ++i)
            {
                for (int j = 0; j < verticesCount; ++j)
                {
                    distance[i, j] = graph[i, j];
                    prev[i, j] = i;
                }
            }

            for (int k = 0; k < verticesCount; ++k)
            {
                for (int i = 0; i < verticesCount; ++i)
                {
                    for (int j = 0; j < verticesCount; ++j)
                    {
                        if (distance[i, k] + distance[k, j] < distance[i, j])
                        {
                            distance[i, j] = distance[i, k] + distance[k, j];
                            prev[i, j] = prev[k, j];
                        }
                    }
                }
            }
            return distance;
        }

        public static int[] Path(int[,] prev, int a, int b)
        {
            List<int> path = new List<int>() { b };
            while (a != b)
            {
                b = prev[a, b];
                path = path.Prepend(b).ToList();
            }
            return path.ToArray();
        }

        public static long PressureRelease(string input)
        {
            Dictionary<string, Valve> valves = ParseInput(input).ToDictionary(k => k.id);
            List<Valve> valveList = valves.Values.ToList();
            List<Valve> targetValves = valveList.Where(v => v.flow_rate > 0).ToList();
            long pressure = 0;
            int flow = 0;

            int totalFlow = 0;
            foreach (Valve v in valveList)
                totalFlow += v.flow_rate;

            int[,] vGraph = new int[valveList.Count, valveList.Count];
            for (int i = 0; i < valveList.Count; i++)
            {
                foreach (Valve v in valveList)
                    v.ResetPath();
                valveList[i].CalculatePaths(valves, 0, valveList[i]);
                for (int j = 0; j < valveList.Count; j++)
                {
                    if (valveList[i].valves.Contains(valveList[j].id) || valveList[i] == valveList[j])
                        vGraph[i, j] = totalFlow - valveList[j].flow_rate;
                    else
                        vGraph[i, j] = INF;
                }
            }

            int[,] distance = FloydWarshall(vGraph, valveList.Count, out int[,] prev);

            Console.WriteLine("Edges between all vertices:");
            Print(vGraph, valveList.Count);
            Console.WriteLine("Shortest distances between every pair of vertices:");
            Print(distance, valveList.Count);
            Console.WriteLine("Previous vertice of each vertice in most efficient path:");
            Print(prev, valveList.Count);

            FinalPath = new int[targetValves.Count+1];
            int length = distance[0, targetValves[0].index];
            for (int i = 0; i < targetValves.Count - 1; i++)
            {
                length += distance[targetValves[i].index, targetValves[i + 1].index];
            }
            Console.WriteLine(length);

            CurrentBest = length;
            int[] path = new int[targetValves.Count];
            Console.WriteLine(CheckPath(distance, targetValves, 0));
            foreach (int i in FinalPath)
                Console.Write($"{i}, ");
            Console.WriteLine();

            return pressure;
        }

        public static int CheckPath(int[,] distances, List<Valve> remaining, int current, int length = 0, int depth = 0)
        {
            if (length > CurrentBest)
            {
                return INF;
            }
            FinalPath[depth] = current;
            if (remaining.Count == 0)
            {
                CurrentBest = length;
                return length;
            }
            foreach (Valve v in remaining)
            {
                length = CheckPath(distances, remaining.NewWithRemoved(v), v.index, length + distances[current, v.index], depth + 1);
            }
            return CurrentBest;
        }

        public static Valve[] ParseInput(string input)
        {
            List<Valve> valves = new List<Valve>();
            int index = 0;
            foreach (string line in input.Split("\r\n"))
            {
                string valve = string.Empty;
                int flow = 0;
                List<string> outs = new List<string>();

                string[] sections = line.Split(' ');
                for (int i = 0; i < sections.Length; i++)
                {
                    if (i == 1)
                        valve = sections[i];
                    if (i == 4)
                        flow = Convert.ToInt32(sections[i].Substring(5, sections[i].Length - 6));
                    if (i > 8)
                    {
                        string s = sections[i].Replace(",", string.Empty);
                        outs.Add(s);
                    }
                }

                Valve v = new Valve(valve, index, flow, outs.ToArray());
                valves.Add(v);
                index++;
            }
            return valves.ToArray();
        }
    }

    internal class Valve : IComparable<Valve>
    {
        public string id;
        public int index;
        public int flow_rate;
        public string[] valves;
        public bool open = false;
        public long cost = -1;
        public long cost_additive = 0;
        public Valve parent;
        public int depth = int.MaxValue;

        public Valve(string id, int index, int flow_rate, string[] valves)
        {
            this.id = id;
            this.index = index;
            this.flow_rate = flow_rate;
            this.valves = valves;
        }

        public void CalculateCost(Dictionary<string, Valve> dict, int depth, Valve v)
        {
            if (depth > 30)
                return;
            long cost = flow_rate * (30 - depth);
            if (this.cost > cost)
                return;
            this.cost = cost;
            parent = v;
            this.depth = depth;
            foreach (string s in valves)
            {
                dict[s].CalculateCost(dict, depth + 1, this);
            }
        }

        public void CalculatePaths(Dictionary<string, Valve> dict, int depth, Valve v)
        {
            if (this.depth < depth)
                return;
            this.depth = depth;
            parent = v;
            foreach (string s in valves)
            {
                dict[s].CalculatePaths(dict, depth + 1, this);
            }
        }

        public void ResetPath()
        {
            this.depth = int.MaxValue;
        }

        public int CompareTo(Valve? other)
        {
            if (other == null)
                return 1;
            long tCost = cost + cost_additive;
            long c_tCost = other.cost + other.cost_additive;
            if (tCost == c_tCost)
                return other.depth - depth;
            if (cost == int.MinValue)
                return -1;
            return (int)(c_tCost - tCost);
        }
    }
}
