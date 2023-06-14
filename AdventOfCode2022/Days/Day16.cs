using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day16
    {
        public static long PressureRelease(string input)
        {
            Dictionary<string, Valve> valves = ParseInput(input).ToDictionary(k => k.id);
            long pressure = 0;
            int flow = 0;

            Valve current_valve = valves["AA"];
            current_valve.CalculateCost(valves, 0, current_valve);
            List<Valve> sortedValves = valves.Values.ToList();

            while (!sortedValves.IsSorted())
            {
                sortedValves.Sort();
                for (int i = 0; i < sortedValves.Count; i++)
                    sortedValves[i].cost_additive = i * sortedValves[i].flow_rate;
            }

            Console.WriteLine("Calculated Costs, displaying sorted list...");
            foreach (Valve v in sortedValves)
                Console.WriteLine($"Valve {v.id} [cost={v.cost}]");

            int index = 0;
            for (int i = 0; i < 30; i++)
            {
                pressure += flow;

                if (index >= sortedValves.Count)
                    continue;

                Valve targetValve = sortedValves[index];
                foreach (Valve v in valves.Values)
                {
                    v.ResetPath();
                }
                current_valve.CalculatePaths(valves, 0, current_valve);

                Console.WriteLine($"Current Valve: [id={current_valve.id}, cost={current_valve.cost}, depth={current_valve.depth}]");
                Console.WriteLine($"Target Valve: [id={targetValve.id}, cost={targetValve.cost}, depth={targetValve.depth}]");

                if (targetValve == current_valve && !current_valve.open)
                {
                    current_valve.open = true;
                    flow += current_valve.flow_rate;
                    Console.WriteLine($"Opening {current_valve.id}");
                    index++;
                    Console.WriteLine("----------------------------------------");
                    continue;
                }
                int a = 0;

                while (targetValve.parent != current_valve && a < 10)
                {
                    targetValve = targetValve.parent;
                    a++;
                }
                if (a >= 10)
                {
                    Console.WriteLine($"Broken, Goal Parent: {targetValve.parent.id}");
                    return 0;
                }
                current_valve = targetValve;
                Console.WriteLine($"Moving to {current_valve.id}");
                Console.WriteLine("----------------------------------------");
            }

            return pressure;
        }

        public static Valve[] ParseInput(string input)
        {
            List<Valve> valves = new List<Valve>();
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

                Valve v = new Valve(valve, flow, outs.ToArray());
                valves.Add(v);
            }
            return valves.ToArray();
        }
    }

    internal class Valve : IComparable<Valve>
    {
        public string id;
        public int flow_rate;
        public string[] valves;
        public bool open = false;
        public long cost = -1;
        public long cost_additive = 0;
        public Valve parent;
        public int depth = int.MaxValue;

        public Valve(string id, int flow_rate, string[] valves)
        {
            this.id = id;
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
