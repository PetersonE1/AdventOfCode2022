using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day13
    {
        public static int PacketOrder(string input)
        {
            string[] pairs = input.Split("\r\n\r\n");

            for (int i = 0; i < pairs.Length; i++)
            {
                string[] split = pairs[i].Split("\r\n");
                List<object> first = ConstructList(ref split[0], 0);
                List<object> second = ConstructList(ref split[1], 0);
            }

            Console.WriteLine("\nRESULTS");
            DisplayResults(list);
            return 0;
        }

        public static List<object> ConstructList(ref string input, int startIndex)
        {
            bool started = false;
            List<object> list = new List<object>();
            string str = string.Empty;

            for (int i = startIndex; i < input.Length; i++)
            {
                char c = input[i];
                if (!started && c == '[')
                {
                    started = true;
                    continue;
                }
                if (!started)
                    continue;

                if (c == '[')
                {
                    list.Add(ConstructList(ref input, i));
                    continue;
                }

                if (c == ',')
                {
                    if (str.Length > 0)
                    {
                        list.Add(Convert.ToInt32(str));
                        str = string.Empty;
                    }
                    continue;
                }

                if (c == ']')
                {
                    if (str.Length > 0)
                    {
                        list.Add(Convert.ToInt32(str));
                    }
                    int length = i - startIndex;
                    input = input.Remove(startIndex, length);
                    Console.WriteLine($"final result: {list.Count}");
                    Console.WriteLine(input);
                    return list;
                }

                str += c;
            }
            Console.WriteLine("No endpoint found");
            return new List<object>();
        }

        public static void DisplayResults(List<object> list)
        {
            foreach (object o in list)
            {
                if (o is int)
                {
                    Console.WriteLine($"Int Detected: {o}");
                }
                if (o is List<object>)
                {
                    Console.WriteLine("List Detected\n[");
                    DisplayResults(o as List<object>);
                    Console.WriteLine("]");
                }
            }
        }
    }
}
