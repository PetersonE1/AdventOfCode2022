using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day5
    {
        public static string CrateEnd(string input, bool firstDay)
        {
            Tuple<string, string> s_Input = input.Split("\n\r\n").ToTuple();
            string[] startingCrates = s_Input.Item1.Split('\n');
            string[] moves = s_Input.Item2.Split('\n');

            string stackLength = startingCrates.Last().Split("   ").Last();
            stackLength = stackLength.Substring(0, stackLength.Length - 1);

            List<char>[] crates = new List<char>[Convert.ToInt32(stackLength)];
            for (int c = 0; c < crates.Length; c++)
            {
                crates[c] = new List<char>();
            }

            for (int i = 0; i < startingCrates.Length - 1; i++)
            {
                for (int j = 0; j < crates.Length; j++)
                {
                    if (ParseCrate(startingCrates[i].Substring(4 * j, 4), out char result))
                    {
                        crates[j].Add(result);
                    }
                }
            }
            foreach (List<char> list in crates) list.Reverse();

            foreach (string i_move in moves)
            {
                Tuple<int, int, int> move = ParseMove(i_move);
                if (firstDay)
                    crates[move.Item2 - 1].MoveTo(crates[move.Item3 - 1], move.Item1);
                else
                    crates[move.Item2 - 1].MoveToContinuous(crates[move.Item3 - 1], move.Item1);
            }

            string output = string.Empty;
            foreach (List<char> stack in crates)
            {
                output += stack.Last();
            }

            return output;
        }

        private static bool ParseCrate(string crate, out char result)
        {
            bool parsing = false;
            foreach (char c in crate)
            {
                if (parsing)
                {
                    result = c;
                    return true;
                }
                if (c == '[')
                    parsing = true;
            }
            result = new char();
            return false;
        }

        private static Tuple<int, int, int> ParseMove(string input)
        {
            string[] s_Input = input.Split(' ');
            Tuple<int, int, int> result = new Tuple<int, int, int>(Convert.ToInt32(s_Input[1]), Convert.ToInt32(s_Input[3]), Convert.ToInt32(s_Input[5]));
            return result;
        }

        public static void MoveTo<T>(this List<T> input, List<T> output, int count)
        {
            for (int i = 0; i < count; i++)
            {
                output.Add(input.Last());
                input.RemoveAt(input.Count - 1);
            }
        }

        public static void MoveToContinuous<T>(this List<T> input, List<T> output, int count)
        {
            List<T> holder = new List<T>();
            for (int i = 0; i < count; i++)
            {
                holder.Add(input.Last());
                input.RemoveAt(input.Count - 1);
            }
            holder.Reverse();
            output.AddRange(holder);
        }
    }
}
