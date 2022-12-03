using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day3
    {
        public static int Sort(string input)
        {
            int sum = 0;
            string[] sacks = input.Split('\n');
            foreach (string sack in sacks)
            {
                string comp1 = sack.Substring(0, sack.Length / 2);
                string comp2 = sack.Substring(sack.Length / 2);
                foreach (char c in comp1)
                {
                    if (comp2.Contains(c))
                    {
                        sum += (int)c % 32 + (char.IsUpper(c) ? 26 : 0);
                        break;
                    }
                }
            }
            return sum;
        }

        public static int Badges(string input)
        {
            int sum = 0;
            string[] sacks = input.Split('\n');
            for (int i = 0; i < sacks.Length; i += 3)
            {
                foreach (char c in sacks[i])
                {
                    if (sacks[i + 1].Contains(c) && sacks[i + 2].Contains(c))
                    {
                        sum += (int)c % 32 + (char.IsUpper(c) ? 26 : 0);
                        break;
                    }
                }
            }
            return sum;
        }
    }
}
