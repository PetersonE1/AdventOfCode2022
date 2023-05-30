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
            int final = 0;

            for (int i = 0; i < pairs.Length; i++)
            {
                string[] split = pairs[i].Split("\r\n");
                List<object> first = ConstructList(ref split[0], 0);
                List<object> second = ConstructList(ref split[1], 0);

                bool result = CompareValues(first, second);
                if (result)
                    final += i + 1;

                DisplayResults(first);
                Console.WriteLine("-----");
                DisplayResults(second);
                Console.WriteLine(result);
            }
            return final;
        }

        public static bool CompareValues(object first, object second)
        {
            if (first is int && second is int)
            {
                return (int)first < (int)second;
            }
            if (first is int && second is List<object>)
            {
                Console.WriteLine($"Converting {first} to list");
                return CompareValues(new List<object>() { first }, second);
            }
            if (first is List<object> && second is int)
            {
                Console.WriteLine($"Converting {second} to list");
                return CompareValues(first, new List<object>() { second });
            }
            if (first is List<object> && second is List<object>)
            {
                List<object> f = (List<object>)first;
                List<object> s = (List<object>)second;
                
                int length = f.Count < s.Count ? f.Count : s.Count;
                for (int j = 0; j < length; j++)
                {
                    Console.WriteLine($"{f[j]}, {s[j]}");
                    if (f[j] is int && s[j] is int && (int)f[j] == (int)s[j])
                        continue;
                    return CompareValues(f[j], s[j]);
                }
                return f.Count < s.Count;
            }
            Console.WriteLine("Unknown data type in comparison");
            return false;
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
