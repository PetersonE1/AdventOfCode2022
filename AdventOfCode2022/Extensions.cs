using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public static class Extensions
    {
        public static int FindIndexOf<T>(this T[] source, T obj)
        {
            int returnVal = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].Equals(obj))
                {
                    returnVal = i;
                    break;
                }
            }
            return returnVal;
        }

        public static int[] FindIdexesOf<T>(this T[] source, T obj)
        {
            List<int> returnVal = new List<int>();
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].Equals(obj))
                {
                    returnVal.Add(i);
                }
            }
            return returnVal?.ToArray() ?? new int[0];
        }

        public static void RemoveRange<T>(this List<T> list, T[] range)
        {
            foreach (T item in range)
                list.Remove(item);
        }

        public static void RemoveRange<T>(this List<T> list, List<T> range)
        {
            foreach (T item in range)
                list.Remove(item);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable) action(item);
        }

        public static string LoadInput(int day, bool first)
        {
            string file = $@"{Directory.GetCurrentDirectory()}\..\..\..\Inputs\Day{day}-{(first ? 0 : 1)}.txt";
            return File.ReadAllText(file);
        }
    }
}
