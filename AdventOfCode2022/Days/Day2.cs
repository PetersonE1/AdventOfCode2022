using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day2
    {
        public static int GameResult(string input)
        {
            char[][] plan = input.Split('\n').Select(s => s.ToCharArray().Where(c => c != ' ').ToArray()).ToArray();
            int score = 0;
            foreach (char[] c in plan)
            {
                switch (c[1])
                {
                    case 'X': c[1] = 'A'; score += 1; break;
                    case 'Y': c[1] = 'B'; score += 2; break;
                    case 'Z': c[1] = 'C'; score += 3; break;
                }
                if (c[0] == c[1])
                {
                    score += 3;
                    continue;
                }
                if (c[0] == 'A')
                {
                    score += (c[1] == 'B' ? 6 : 0);
                    continue;
                }
                if (c[0] == 'B')
                {
                    score += (c[1] == 'A' ? 0 : 6);
                    continue;
                }
                if (c[0] == 'C')
                {
                    score += (c[1] == 'A' ? 6 : 0);
                    continue;
                }
            }
            return score;
        }

        public static int TrueGameResult(string input)
        {
            char[][] plan = input.Split('\n').Select(s => s.ToCharArray().Where(c => c != ' ').ToArray()).ToArray();
            int score = 0;
            foreach (char[] c in plan)
            {
                switch (c[1])
                {
                    case 'X':
                        { 
                            switch (c[0])
                            {
                                case 'A': score += 3; break;
                                case 'B': score += 1; break;
                                case 'C': score += 2; break;
                            }
                        } 
                        break;
                    case 'Y':
                        {
                            switch (c[0])
                            {
                                case 'A': score += 1; break;
                                case 'B': score += 2; break;
                                case 'C': score += 3; break;
                            }
                        }
                        score += 3; break;
                    case 'Z': 
                        {
                            switch (c[0])
                            {
                                case 'A': score += 2; break;
                                case 'B': score += 3; break;
                                case 'C': score += 1; break;
                            }
                        }
                        score += 6; break;
                }
            }
            return score;
        }
    }
}
