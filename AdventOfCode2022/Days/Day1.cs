using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day1
    {
        public static int CalculateMostCalories(string input)
        {
            int[] calories = input.Split("\n\r\n").Select(s => s.Split('\n').Select(n => Convert.ToInt32(n)).Sum()).ToArray();
            return calories.FindIndexOf(calories.Max()) + 1;
        }
    }
}
