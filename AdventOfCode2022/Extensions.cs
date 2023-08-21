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
        public static int FindIndexOf<T>(this IEnumerable<T> source, T obj)
        {
            int returnVal = 0;
            for (int i = 0; i < source.Count(); i++)
            {
                if (source.ElementAt(i).Equals(obj))
                {
                    returnVal = i;
                    break;
                }
            }
            return returnVal;
        }

        public static int[] FindIdexesOf<T>(this IEnumerable<T> source, T obj)
        {
            List<int> returnVal = new List<int>();
            for (int i = 0; i < source.Count(); i++)
            {
                if (source.ElementAt(i).Equals(obj))
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

        public static List<T> NewWithRemoved<T>(this List<T> list, T obj)
        {
            List<T> values = new List<T>(list);
            values.Remove(obj);
            return values;
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

        public static T[] Combine<T>(this T[][] input)
        {
            List<T> values = new List<T>();
            foreach (T[] value in input)
                values.AddRange(value);
            return values.ToArray();
        }

        public static List<T> Slice<T>(this List<T> input, int index)
        {
            int count = input.Count - index;
            return input.GetRange(index, count);
        }

        public static Dictionary<TKey, TValue> ToDictionarySafe<TKey, TValue>(this IEnumerable<TValue> input, Func<TValue, TKey> keySelector) where TKey : notnull
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            foreach (TValue item in input)
            {
                dictionary.TryAdd(keySelector.Invoke(item), item);
            }
            return dictionary;
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

        public static bool IsSorted<T>(this List<T> list) where T : IComparable<T>
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].CompareTo(list[i - 1]) < 0)
                    return false;
            }
            return true;
        }

        public static int UnitFunc(double value)
        {
            return value < 0 ? 0 : 1;
        }

        public static string LoadInput(string fileName, bool testFile)
        {
            string file;
            if (!testFile)
                file = $@"{Directory.GetCurrentDirectory()}\..\..\..\Inputs\{fileName}.txt";
            else
                file = $@"{Directory.GetCurrentDirectory()}\..\..\..\Inputs\TestData\{fileName}T.txt";
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
            return $@"[{x}, {y}]";
        }

        public int CompareTo(object? obj)
        {
            if (!(obj is Vector2Int))
                return 0;
            Vector2Int v = (Vector2Int)obj;
            if (x != v.x)
                return x - v.x;
            return y - v.y;
        }

        public double Distance(Vector2Int v) 
        {
            int xdiff = Math.Abs(v.x - x);
            int ydiff = Math.Abs(v.y - y);
            return Math.Sqrt((xdiff * xdiff) + (ydiff * ydiff));
        }

        public int Manhattan(Vector2Int v)
        {
            int xdiff = Math.Abs(v.x - x);
            int ydiff = Math.Abs(v.y - y);
            return xdiff + ydiff;
        }
    }
}
