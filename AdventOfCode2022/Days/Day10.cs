using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day10
    {
        public static int registry = 1;

        public static int SignalStrength(string input, int startCycle, int cycleStep, int maxCycle)
        {
            List<Tuple<int, string>> commandBuffer = PopulateBuffer(input);
            int index = 0;
            int nextCheck = startCycle;
            int regSum = 0;

            int cycle = 0;
            while (index < commandBuffer.Count)
            {
                cycle++;

                if (cycle > maxCycle)
                    return regSum;

                if (cycle == nextCheck)
                {
                    nextCheck += cycleStep;
                    regSum += registry * cycle;
                    Console.WriteLine(registry);
                }

                if (cycle == commandBuffer[index].Item1)
                {
                    ProcessCommand(commandBuffer[index].Item2);
                    index++;
                }
            }

            Console.WriteLine(registry);
            return regSum;
        }

        public static string DrawInput(string input)
        {
            List<Tuple<int, string>> commandBuffer = PopulateBuffer(input);
            List<string> image = new List<string>();
            int index = 0;
            int crt_pos = 0;

            int cycle = 0;
            string displayLine = string.Empty;
            while (index < commandBuffer.Count || crt_pos % 40f != 0f)
            {
                cycle++;

                if (crt_pos >= registry-1 && crt_pos <= registry + 1)
                {
                    displayLine += "#";
                }
                else
                {
                    displayLine += ".";
                }

                crt_pos++;

                if (crt_pos % 40f == 0f)
                {
                    crt_pos = 0;
                    image.Add(displayLine);
                    displayLine = string.Empty;
                }

                if (cycle == commandBuffer[index].Item1)
                {
                    ProcessCommand(commandBuffer[index].Item2);
                    index++;
                }
            }

            return string.Join('\n', image);
        }

        private static void ProcessCommand(string command)
        {
            if (command.Contains("noop"))
                return;
            if (command.Contains("addx"))
            {
                registry += Convert.ToInt32(command.Split(' ')[1]);
            }
        }

        private static List<Tuple<int, string>> PopulateBuffer(string input)
        {
            List<Tuple<int, string>> commandBuffer = new List<Tuple<int, string>>();
            int cycleLength = 0;

            foreach (string line in input.Split('\n'))
            {
                if (line.Contains("noop"))
                    cycleLength += 1;
                if (line.Contains("addx"))
                    cycleLength += 2;
                commandBuffer.Add(new Tuple<int, string>(cycleLength, line));
            }

            return commandBuffer;
        }
    }
}
