using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day4
    {
        public static int FullyContains(string input)
        {
            Tuple<string, string>[] pairs = input.Split('\n').Select(s => s.Split(',').ToTuple()).ToArray();
            int count = 0;
            foreach (Tuple<string, string> pair in pairs)
            {
                int[] r1 = pair.Item1.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
                int[] r2 = pair.Item2.Split('-').Select(n => Convert.ToInt32(n)).ToArray();

                Range range1 = new Range(r1[0], r1[1]);
                Range range2 = new Range(r2[0], r2[1]);

                if (range1.Contains(range2) || range2.Contains(range1)) count++;
            }
            return count;
        }

        public static int Overlaps(string input)
        {
            Tuple<string, string>[] pairs = input.Split('\n').Select(s => s.Split(',').ToTuple()).ToArray();
            int count = 0;
            foreach (Tuple<string, string> pair in pairs)
            {
                int[] r1 = pair.Item1.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
                int[] r2 = pair.Item2.Split('-').Select(n => Convert.ToInt32(n)).ToArray();

                Range range1 = new Range(r1[0], r1[1]);
                Range range2 = new Range(r2[0], r2[1]);

                if (range1.Overlaps(range2)) count++;
            }
            return count;
        }
    }
}
