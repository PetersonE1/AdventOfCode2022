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
                if (i == 0)
                    current_valve = valves[current_valve.valves.Last()];
                if (i == 1)
                    flow += current_valve.flow_rate;
                if (i == 2)
                    current_valve = valves[current_valve.valves.First()];
                if (i == 3)
                    flow += current_valve.flow_rate;
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

    internal class Valve
    {
        public string id;
        public int flow_rate;
        public string[] valves;

        public Valve(string id, int flow_rate, string[] valves)
        {
            this.id = id;
            this.flow_rate = flow_rate;
            this.valves = valves;
        }
    }
}
