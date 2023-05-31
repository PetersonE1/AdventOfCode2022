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
            bool test = false;
            if (args.Length > 2)
                test = Convert.ToBoolean(args[2]);
            switch (args[0])
            {
                case "1": RunDay1(isFirst, test); break;
                case "2": RunDay2(isFirst, test); break;
                case "3": RunDay3(isFirst, test); break;
                case "4": RunDay4(isFirst, test); break;
                case "5": RunDay5(isFirst, test); break;
                case "6": RunDay6(isFirst, test); break;
                case "7": RunDay7(isFirst, test); break;
                case "8": RunDay8(isFirst, test); break;
                case "9": RunDay9(isFirst, test); break;
                case "10": RunDay10(isFirst, test); break;
                case "11": RunDay11(isFirst, test); break;
                case "12": RunDay12(isFirst, test); break;
                case "13": RunDay13(isFirst, test); break;
                case "14": RunDay14(isFirst, test); break;
                case "15": RunDay15(isFirst, test); break;
                default: break;
            }
        }

        private static void RunDay1(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day1", runTest);
            Console.WriteLine(Days.Day1.CalculateMostCalories(input, firstDay));
        }

        private static void RunDay2(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day2", runTest);
            if (firstDay)
                Console.WriteLine(Days.Day2.GameResult(input));
            else
                Console.Write(Days.Day2.TrueGameResult(input));
        }

        private static void RunDay3(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day3", runTest);
            if (firstDay)
                Console.WriteLine(Days.Day3.Sort(input));
            else
                Console.WriteLine(Days.Day3.Badges(input));
        }

        private static void RunDay4(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day4", runTest);
            if (firstDay)
                Console.WriteLine(Days.Day4.FullyContains(input));
            else
                Console.WriteLine(Days.Day4.Overlaps(input));
        }

        private static void RunDay5(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day5", runTest);
            Console.WriteLine(Days.Day5.CrateEnd(input, firstDay));
        }

        private static void RunDay6(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day6", runTest);
            if (firstDay)
                Console.WriteLine(Days.Day6.StartOfPacket(input, 4));
            else
                Console.WriteLine(Days.Day6.StartOfPacket(input, 14));
        }

        private static void RunDay7(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day7", runTest);
            Console.WriteLine(Days.Day7.DirectorySum(input, firstDay));
        }

        private static void RunDay8(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day8", runTest);
            if (firstDay)
                Console.WriteLine(Days.Day8.CheckVisibility(input));
            else
                Console.WriteLine(Days.Day8.ScenicScore(input));
        }

        private static void RunDay9(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day9", runTest);
            if (firstDay)
                Console.WriteLine(Days.Day9.SimulateRope(input, 2));
            else
                Console.WriteLine(Days.Day9.SimulateRope(input, 10));
        }

        private static void RunDay10(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day10", runTest);
            if (firstDay)
                Console.WriteLine(Days.Day10.SignalStrength(input, 20, 40, 220));
            else
                Console.WriteLine(Days.Day10.DrawInput(input));
        }

        private static void RunDay11(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day11", runTest);
            if (firstDay)
                Console.WriteLine(Days.Day11.MonkeyBusiness(input, 20, true));
            else
                Console.WriteLine(Days.Day11.MonkeyBusiness(input, 10000, false));
        }

        private static void RunDay12(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day12", runTest);
            Console.WriteLine(Days.Day12.FewestSteps(input, firstDay));
        }

        private static void RunDay13(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day13", runTest);
            if (firstDay)
                Console.WriteLine(Days.Day13.PacketOrder(input));
            else
                Console.WriteLine(Days.Day13.SortPackets(input));
        }
        
        private static void RunDay14(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day14", runTest);
            Console.WriteLine(Days.Day14.RestingSand(input, firstDay));
        }

        private static void RunDay15(bool firstDay, bool runTest = false)
        {
            string input = Extensions.LoadInput("Day15", runTest);
            int line = runTest ? 10 : 2000000;
            int area = runTest ? 20 : 4000000;
            if (firstDay)
                Console.WriteLine(Days.Day15.BeaconMissing(input, line));
            else
                Console.WriteLine(Days.Day15.FindBeacon(input, area));
        }
    }
}