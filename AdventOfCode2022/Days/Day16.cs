using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day16
    {
        const int INF = 99999;

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

        public static long PressureRelease(string input, bool firstDay)
        {
            Dictionary<string, Valve> valves = ParseInput(input).ToDictionary(k => k.id);
            List<Valve> valveList = valves.Values.ToList();
            long pressure = 0;

            int[,] vGraph = new int[valveList.Count, valveList.Count];
            for (int i = 0; i < valveList.Count; i++)
            {
                for (int j = 0; j < valveList.Count; j++)
                {
                    if (valveList[i] == valveList[j])
                        vGraph[i, j] = 0;
                    else if (valveList[i].valves.Contains(valveList[j].id))
                        vGraph[i, j] = 1;
                    else
                        vGraph[i, j] = INF;
                }
            }

            int[,] distance = FloydWarshall(vGraph, valveList.Count, out int[,] prev);

            for (int i = 0; i < valveList.Count; i++)
            {
                for (int j = 0; j < valveList.Count; j++)
                {
                    valveList[i].edges.Add(valveList[j].id, distance[i, j] + 1);
                }
            }

            Console.WriteLine("Edges between all vertices:");
            Print(vGraph, valveList.Count);
            Console.WriteLine("Shortest distances between every pair of vertices:");
            Print(distance, valveList.Count);
            Console.WriteLine("Previous vertice of each vertice in most efficient path:");
            Print(prev, valveList.Count);

            valves = valves.Where(v => v.Value.flow_rate > 0 || v.Value.id == "AA").ToDictionary(v => v.Key, v => v.Value);
            if (firstDay)
                return DepthFirst(valves, "AA", 30);

            return Elephant(valves, "AA", 26);
        }

        private static long Elephant(Dictionary<string, Valve> valves, string start, int rounds)
        {
            Valve start_node = valves[start];

            List<Valve> unopened = valves.Values.Where(v => v.id != start).ToList();

            int split_count = unopened.Count / 2;
            int split_mod = unopened.Count % 2;
            Valve[][] splits = combinations(unopened, split_count + split_mod);

            List<long> best_flow = new List<long>();

            for (int i = 0; i < splits.Length; i++)
            {
                List<Valve> temp = unopened.ToList();
                for (int j = 0; j < splits[i].Length; j++)
                {
                    int index = temp.FindIndex(n => n.id == splits[i][j].id);
                    if (index > -1)
                    {
                        temp.RemoveAt(index);
                    }
                }

                Dictionary<string, Valve> splitsDict = splits[i].ToDictionarySafe(k => k.id);
                Dictionary<string, Valve> tempDict = temp.ToDictionarySafe(k => k.id);
                splitsDict.Add(start, start_node);
                tempDict.Add(start, start_node);

                best_flow.Add(
                    DepthFirst(splitsDict, start, rounds)
                    + DepthFirst(tempDict, start, rounds));
            }

            best_flow.Sort((a, b) => (int)(b - a));

            return best_flow[0];
        }

        private static long DepthFirst(Dictionary<string, Valve> valves, string start, int rounds)
        {
            Queue<Step> queue = new Queue<Step>();
            queue.Enqueue(new Step()
            {
                valve = valves[start],
                visited = new List<string>(),
                calculation = new Step.Calculation()
            });

            Step winner = queue.Peek();
            while (queue.Count > 0)
            {
                Step current = queue.Dequeue();
                current.visited.Add(current.valve.id);

                if (current.calculation.total_flow > winner.calculation.total_flow)
                    winner = current;

                foreach (Valve edge in valves.Values)
                {
                    if (current.visited.Contains(edge.id)
                        || current.valve.id == edge.id
                        || current.calculation.steps + current.valve.edges[edge.id] > rounds
                        )   continue;

                    Step.Calculation new_calculation = new Step.Calculation();
                    new_calculation.steps = current.calculation.steps + current.valve.edges[edge.id];

                    new_calculation = calc_flow(current, edge, rounds, new_calculation);

                    if (new_calculation.total_flow < winner.calculation.total_flow
                        && new_calculation.steps >= winner.calculation.steps)
                        continue;

                    queue.Enqueue(new Step()
                    {
                        valve = edge,
                        visited = current.visited.ToList(),
                        calculation = new_calculation
                    });
                }
            }
            return winner.calculation.total_flow;
        }

        private static Step.Calculation calc_flow(Step current, Valve edge, int rounds, Step.Calculation calculation)
        {
            calculation.flow = (current.calculation.flow_rate * current.valve.edges[edge.id]) + current.calculation.flow;

            calculation.flow_rate = current.calculation.flow_rate + edge.flow_rate;
            calculation.total_flow = (calculation.flow_rate * (rounds - calculation.steps)) + calculation.flow;

            return calculation;
        }

        private static Valve[][] combinations(List<Valve> unopened, int length)
        {
            if (length == 0) return new Valve[1][];
            List<Valve[]> result = new List<Valve[]>();
            for (int i = 0; i <= unopened.Count - length; i++)
            {

                Valve[][] sub_result = combinations(unopened.Slice(i + 1), length - 1);
                foreach (Valve[] combination in sub_result)
                {
                    List<Valve> valves = new List<Valve>
                    {
                        unopened[i]
                    };

                    if (combination != null)
                        foreach (Valve v in combination)
                            valves.Add(v);

                    result.Add(valves.ToArray());
                }
            }
            return result.ToArray();
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

    internal class Valve
    {
        public string id;
        public int index;
        public int flow_rate;
        public string[] valves;
        public Valve parent;
        public Dictionary<string, int> edges = new Dictionary<string, int>();

        public Valve(string id, int index, int flow_rate, string[] valves)
        {
            this.id = id;
            this.index = index;
            this.flow_rate = flow_rate;
            this.valves = valves;
        }
    }

    internal class Step
    {
        public Valve valve;
        public List<string> visited;
        public Calculation calculation;

        public class Calculation
        {
            public int steps = 0;
            public int flow_rate = 0;
            public long flow = 0;
            public long total_flow = 0;
        }
    }
}
