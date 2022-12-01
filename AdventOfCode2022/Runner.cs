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
                default: break;
            }
        }

        private static void RunDay1(bool firstDay)
        {
            string input = Extensions.LoadInput("Day1");
            Console.WriteLine(Days.Day1.CalculateMostCalories(input, firstDay));
        }
    }
}