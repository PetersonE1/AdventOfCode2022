using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public static class Runner
    {
        public static void Main(string[] args)
        {
            bool isFirst = Convert.ToBoolean(args[1]);
            switch (args[0])
            {
                case "1": RunDay1(isFirst); break;
                case "2": RunDay2(isFirst); break;
                case "3": RunDay3(isFirst); break;
                case "4": RunDay4(isFirst); break;
                default: break;
            }
        }

        private static void RunDay1(bool firstDay)
        {
            string input = Extensions.LoadInput("Day1");
            Console.WriteLine(Days.Day1.CalculateMostCalories(input, firstDay));
        }

        private static void RunDay2(bool firstDay)
        {
            string input = Extensions.LoadInput("Day2");
            if (firstDay)
                Console.WriteLine(Days.Day2.GameResult(input));
            else
                Console.Write(Days.Day2.TrueGameResult(input));
        }

        private static void RunDay3(bool firstDay)
        {
            string input = Extensions.LoadInput("Day3");
            if (firstDay)
                Console.WriteLine(Days.Day3.Sort(input));
            else
                Console.WriteLine(Days.Day3.Badges(input));
        }

        private static void RunDay4(bool firstDay)
        {
            string input = Extensions.LoadInput("Day4");
            if (firstDay)
                Console.WriteLine(Days.Day4.FullyContains(input));
            else
                Console.WriteLine(Days.Day4.Overlaps(input));
        }
    }
}