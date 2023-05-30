using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

                int result = CompareValues(first, second);
                if (result == 1)
                    final += i + 1;

                /*DisplayResults(first);
                Console.WriteLine("-----");
                DisplayResults(second);
                Console.WriteLine(result);*/
            }
            return final;
        }

        public static int SortPackets(string input)
        {
            input = input.Replace("\r\n\r\n", "\r\n");
            input += "\r\n[[2]]\r\n[[6]]";
            Dictionary<List<object>, string> lines = input.Split("\r\n").ToDictionary(n => ConstructList(ref n, 0));
            int final = 0;

            ImmutableSortedDictionary<List<object>, string> sortedLines = lines.ToImmutableSortedDictionary(new Day13Sort());
            for (int i = 0; i < sortedLines.Count; i++)
            {
                string s = sortedLines.ElementAt(i).Value;
                if (s == "[[2]]")
                    final = i + 1;
                if (s == "[[6]]")
                    final *= i + 1;
                //Console.WriteLine(s);
            }
            return final;
        }

        public static int CompareValues(object first, object second)
        {
            if (first is int && second is int)
            {
                if ((int)first == (int)second)
                    return 2;
                return (int)first < (int)second ? 1 : 0;
            }
            if (first is int && second is List<object>)
            {
                //Console.WriteLine($"Converting {first} to list");
                return CompareValues(new List<object>() { first }, second);
            }
            if (first is List<object> && second is int)
            {
                //Console.WriteLine($"Converting {second} to list");
                return CompareValues(first, new List<object>() { second });
            }
            if (first is List<object> && second is List<object>)
            {
                List<object> f = (List<object>)first;
                List<object> s = (List<object>)second;
                
                int length = f.Count < s.Count ? f.Count : s.Count;
                for (int j = 0; j < length; j++)
                {
                    //Console.WriteLine($"{f[j]}, {s[j]}");
                    int result = CompareValues(f[j], s[j]);
                    if (result != 2)
                        return result;
                }
                if (f.Count == s.Count)
                    return 2;
                return f.Count < s.Count ? 1 : 0;
            }
            Console.WriteLine("Unknown data type in comparison");
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

    internal class Day13Sort : IComparer<List<object>>
    {
        public int Compare(List<object> a, List<object> b)
        {
            if (a == null || b == null)
                return -1;
            int result = Day13.CompareValues(a, b);
            if (result == 1)
                return -1;
            if (result == 0)
            {
                return 1;
            }
            return 1;
        }
    }
}
