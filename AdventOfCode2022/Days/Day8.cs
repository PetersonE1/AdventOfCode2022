using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day8
    {
        public static int CheckVisibility(string input)
        {
            int[][] trees = input.Split("\r\n").Select(s => s.Select(n => Convert.ToInt32(n.ToString())).ToArray()).ToArray();
            int[][] visible = new int[trees.Length][];
            for (int i = 0; i < visible.Length; i++)
            {
                visible[i] = new int[trees[i].Length];
                for (int j = 0; j < visible[i].Length; j++)
                {
                    visible[i][j] = 0;
                }
            }

            for (int row = 0; row < trees.Length; row++)
            {
                int tallest = -1;
                for (int col = 0; col < trees[row].Length; col++)
                {
                    if (trees[row][col] > tallest)
                    {
                        tallest = trees[row][col];
                        visible[row][col] = 1;
                    }
                }
            }

            for (int row = 0; row < trees.Length; row++)
            {
                int tallest = -1;
                for (int col = trees[row].Length - 1; col >= 0; col--)
                {
                    if (trees[row][col] > tallest)
                    {
                        tallest = trees[row][col];
                        visible[row][col] = 1;
                    }
                }
            }

            for (int col = 0; col < trees[0].Length; col++)
            {
                int tallest = -1;
                for (int row = 0; row < trees.Length; row++)
                {
                    if (trees[row][col] > tallest)
                    {
                        tallest = trees[row][col];
                        visible[row][col] = 1;
                    }
                }
            }

            for (int col = 0; col < trees[0].Length; col++)
            {
                int tallest = -1;
                for (int row = trees.Length - 1; row >= 0; row--)
                {
                    if (trees[row][col] > tallest)
                    {
                        tallest = trees[row][col];
                        visible[row][col] = 1;
                    }
                }
            }

            int sum = 0;
            for (int i = 0; i < visible.Length; i++)
            {
                for (int j = 0; j < visible[i].Length; j++)
                {
                    if (visible[i][j] == 1)
                        sum++;
                }
            }

            return sum;
        }

        public static int ScenicScore(string input)
        {
            int[][] trees = input.Split("\r\n").Select(s => s.Select(n => Convert.ToInt32(n.ToString())).ToArray()).ToArray();
            int highestScore = 0;

            for (int row = 0; row < trees.Length; row++)
            {
                for (int col = 0; col < trees[row].Length; col++)
                {
                    int score = CheckScore(trees, row, col);
                    if (score > highestScore) highestScore = score;
                }
            }

            return highestScore;
        }

        private static int CheckScore(int[][] trees, int x, int y)
        {
            int scoreXPos = 0;
            for (int xPos = 1; xPos < trees.Length; xPos++)
            {
                try
                {
                    if (!(trees[x + xPos][y] >= trees[x][y]))
                        scoreXPos++;
                    else
                    {
                        scoreXPos++;
                        break;
                    }
                }
                catch
                {
                    break;
                }
            }

            int scoreXNeg = 0;
            for (int xNeg = 1; xNeg < trees.Length; xNeg++)
            {
                try
                {
                    if (!(trees[x - xNeg][y] >= trees[x][y]))
                        scoreXNeg++;
                    else
                    {
                        scoreXNeg++;
                        break;
                    }
                }
                catch
                {
                    break;
                }
            }

            int scoreYPos = 0;
            for (int yPos = 1; yPos < trees.Length; yPos++)
            {
                try
                {
                    if (!(trees[x][y + yPos] >= trees[x][y]))
                        scoreYPos++;
                    else
                    {
                        scoreYPos++;
                        break;
                    }
                }
                catch
                {
                    break;
                }
            }

            int scoreYNeg = 0;
            for (int yNeg = 1; yNeg < trees.Length; yNeg++)
            {
                try
                {
                    if (!(trees[x][y - yNeg] >= trees[x][y]))
                        scoreYNeg++;
                    else
                    {
                        scoreYNeg++;
                        break;
                    }
                }
                catch
                {
                    break;
                }
            }

            return scoreXPos * scoreXNeg * scoreYPos * scoreYNeg;
        }
    }
}
