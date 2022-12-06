using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day6
    {
        public static int StartOfPacket(string input, int markerLength)
        {
            char[] buffer;
            for (int i = 0; i < input.Length - 3; i++)
            {
                buffer = input.Substring(i, markerLength).ToCharArray();
                if (buffer.ContainsDuplicate())
                {
                    continue;
                }
                return i + markerLength;
            }
            return 0;
        }

        private static bool ContainsDuplicate<T>(this T[] array)
        {
            List<T> holder = new List<T>();
            foreach (T item in array)
            {
                if (holder.Contains(item))
                    return true;
                holder.Add(item);
            }
            return false;
        }
    }
}
