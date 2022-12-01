using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day1
    {
        public static int CalculateMostCalories(string input, bool first)
        {
            int[] calories = input.Split("\n\r\n").Select(s => s.Split('\n').Select(n => Convert.ToInt32(n)).Sum()).ToArray();
            SortedSet<int> sortedCalories = new SortedSet<int>();
            calories.ForEach(n => sortedCalories.Add(n));
            calories = sortedCalories.ToArray();

            if (first)
                return calories[^1];
            else
                return calories[^1] + calories[^2] + calories[^3];
        }
    }
}
