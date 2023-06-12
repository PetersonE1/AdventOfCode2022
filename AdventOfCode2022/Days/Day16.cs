using System;
using System.Collections.Generic;
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
            for (int i = 0; i < 30; i++)
            {
                pressure += flow;

                foreach (Valve v in valves.Values)
                    v.ClearScore();

                Console.WriteLine($"Current Valve: {current_valve.id}");

                current_valve.CalculateScore(valves, 0, current_valve);
                Valve goal = valves.Values.Max();

                Console.WriteLine($"Target Valve: {goal.id} [score={goal.score}]");

                if (goal == current_valve)
                {
                    Console.WriteLine(0);
                    current_valve.open = true;
                    flow += current_valve.flow_rate;
                    Console.WriteLine($"Opening {current_valve.id}");
                    Console.WriteLine("----------------------------------------");
                    continue;
                }
                int a = 0;
                while (goal.parent != current_valve && a < 10)
                {
                    goal = goal.parent;
                    a++;
                }
                if (a >= 10)
                {
                    Console.WriteLine($"Broken, Goal Parent: {goal.parent.id}");
                    return 0;
                }
                current_valve = goal;
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
        public long score = 0;
        public Valve parent;

        public Valve(string id, int flow_rate, string[] valves)
        {
            this.id = id;
            this.flow_rate = flow_rate;
            this.valves = valves;
        }

        public void CalculateScore(Dictionary<string, Valve> dict, int depth, Valve v)
        {
            long score = flow_rate - (depth * depth);
            if (this.score > score)
                return;
            this.score = score;
            parent = v;
            foreach (string s in valves)
            {
                dict[s].CalculateScore(dict, depth + 1, this);
            }
            if (open)
                this.score = 0;
        }

        public void ClearScore()
        {
            score = 0;
        }

        public int CompareTo(Valve? other)
        {
            if (other == null)
                return 1;
            return (int)(score - other.score);
        }
    }
}
