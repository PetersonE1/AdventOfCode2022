using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public static Tuple<T, T> ToTuple<T>(this IEnumerable<T> enumerable)
        {
            T[] array = enumerable.ToArray();
            return new Tuple<T, T>(array[0], array[1]);
        }

        public static bool Contains(this Range r1, Range r2)
        {
            if (r1.Start.Value >= r2.Start.Value && r1.End.Value <= r2.End.Value)
            {
                return true;
            }
            return false;
        }

        public static bool Overlaps(this Range r1, Range r2)
        {
            if ((r1.Start.Value >= r2.Start.Value && r1.Start.Value <= r2.End.Value) || (r1.End.Value >= r2.Start.Value && r1.End.Value <= r2.End.Value))
            {
                return true;
            }
            if ((r2.Start.Value >= r1.Start.Value && r2.Start.Value <= r1.End.Value) || (r2.End.Value >= r1.Start.Value && r2.End.Value <= r1.End.Value))
            {
                return true;
            }
            return false;
        }

        public static T[] CloneExcluding<T>(this T[] array, int[] indexes)
        {
            List<T> values = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                if (!indexes.Contains(i))
                    values.Add(array[i]);
            }
            return values.ToArray();
        }

        public static string ToHumanString<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.Count() < 1)
                return "[]";

            string s = "[";
            foreach (T item in enumerable)
            {
                s += $"{item}, ";
            }
            s = s.Remove(s.Length - 2) + "]";
            return s;
        }

        public static string LoadInput(string fileName)
        {
            string file = $@"{Directory.GetCurrentDirectory()}\..\..\..\Inputs\{fileName}.txt";
            return File.ReadAllText(file);
        }
    }

    public struct Vector2Int : IComparable
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // Operator Overloads
        public static Vector2Int operator +(Vector2Int a) => a;
        public static Vector2Int operator -(Vector2Int a) => new Vector2Int(-a.x, -a.y);

        public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.x + b.x, a.y + b.y);
        public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.x - b.x, a.y - b.y);
        public static Vector2Int operator *(Vector2Int a, Vector2Int b) => new Vector2Int(a.x * b.x, a.y * b.y);
        public static Vector2Int operator /(Vector2Int a, Vector2Int b) => new Vector2Int(a.x / b.x, a.y / b.y);

        public static Vector2Int operator +(Vector2Int a, double b) => new Vector2Int((int)(a.x + b), (int)(a.y + b));
        public static Vector2Int operator -(Vector2Int a, double b) => new Vector2Int((int)(a.x - b), (int)(a.y - b));
        public static Vector2Int operator *(Vector2Int a, double b) => new Vector2Int((int)(a.x * b), (int)(a.y * b));
        public static Vector2Int operator /(Vector2Int a, double b) => new Vector2Int((int)(a.x / b), (int)(a.y / b));

        public static Vector2Int operator +(Vector2Int a, float b) => new Vector2Int((int)(a.x + b), (int)(a.y + b));
        public static Vector2Int operator -(Vector2Int a, float b) => new Vector2Int((int)(a.x - b), (int)(a.y - b));
        public static Vector2Int operator *(Vector2Int a, float b) => new Vector2Int((int)(a.x * b), (int)(a.y * b));
        public static Vector2Int operator /(Vector2Int a, float b) => new Vector2Int((int)(a.x / b), (int)(a.y / b));

        public static Vector2Int operator +(Vector2Int a, int b) => new Vector2Int(a.x + b, a.y + b);
        public static Vector2Int operator -(Vector2Int a, int b) => new Vector2Int(a.x - b, a.y - b);
        public static Vector2Int operator *(Vector2Int a, int b) => new Vector2Int(a.x * b, a.y * b);
        public static Vector2Int operator /(Vector2Int a, int b) => new Vector2Int(a.x / b, a.y / b);

        public static bool operator ==(Vector2Int a, Vector2Int b) => Vector2Int.Equals(a, b);
        public static bool operator !=(Vector2Int a, Vector2Int b) => !Vector2Int.Equals(a, b);

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (!(obj is Vector2Int))
            {
                return false;
            }
            Vector2Int v = (Vector2Int)obj;
            return x == v.x && y == v.y;
        }

        public override int GetHashCode()
        {
            return ShiftAndWrap(x.GetHashCode(), 2) ^ y.GetHashCode();
        }

        private int ShiftAndWrap(int value, int positions)
        {
            positions = positions & 0x1F;

            // Save the existing bit pattern, but interpret it as an unsigned integer.
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            // Preserve the bits to be discarded.
            uint wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }

        public override string ToString()
        {
            return $@"{x}, {y}";
        }

        public int CompareTo(object? obj)
        {
            if (!(obj is Vector2Int))
                return 0;
            Vector2Int v = (Vector2Int)obj;
            if (x != v.x)
                return v.x - x;
            return v.y - y;
        }
    }
}
