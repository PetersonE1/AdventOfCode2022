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

                current_valve.visited = true;
                List<Valve> toVisit = new List<Valve>();
                foreach (string s in current_valve.valves)
                    toVisit.Add(valves[s]);
                toVisit.Sort(new SortValveByPaths());
                Console.WriteLine(current_valve.id);

                List<Valve> toVisitFiltered = toVisit.Where(n => !n.open).ToList();
                if (!current_valve.open && (toVisitFiltered != null && toVisitFiltered.Count > 0 && current_valve.flow_rate > toVisitFiltered.Max().flow_rate))
                {
                    current_valve.open = true;
                    flow += current_valve.flow_rate;
                    continue;
                }
                if (!current_valve.open && (toVisitFiltered == null || toVisitFiltered.Count == 0))
                {
                    current_valve.open = true;
                    flow += current_valve.flow_rate;
                    continue;
                }
                bool toContinue = false;
                foreach (Valve v in toVisit)
                {
                    if (!v.visited)
                    {
                        current_valve = v;
                        toContinue = true;
                        break;
                    }
                }
                if (toContinue)
                    continue;

                IEnumerable<Valve> closedValves = valves.Values.Where(v => !v.open && v.flow_rate > 0);
                if (closedValves.Count() == 1)
                {
                    if (current_valve.valves.Contains(closedValves.First().id))
                    {
                        current_valve = closedValves.First();
                        continue;
                    }
                }

                current_valve = toVisit.First();
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
        public bool visited = false;

        public Valve(string id, int flow_rate, string[] valves)
        {
            this.id = id;
            this.flow_rate = flow_rate;
            this.valves = valves;
        }

        public int CompareTo(Valve? other)
        {
            if (other == null)
                return 1;
            return flow_rate - other.flow_rate;
        }
    }

    internal class SortValveByPaths : IComparer<Valve>
    {
        public int Compare(Valve? x, Valve? y)
        {
            return y.valves.Length - x.valves.Length;
        }
    }
}
