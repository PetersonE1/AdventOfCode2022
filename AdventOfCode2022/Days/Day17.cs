using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day17
    {
        public static int Tetris(string input)
        {
            Rock[] RockTypes = InitializeRocks();

            for (int i = 0; i <= 2022; i++)
            {
                int jetIndex = i;
                while (jetIndex >= input.Length)
                    jetIndex -= input.Length;

                int rockIndex = i;
                while (rockIndex >= RockTypes.Length)
                    rockIndex -= RockTypes.Length;

                bool landed = false;
                while (!landed)
                {

                }
            }

            return 0;
        }

        private static Rock[] InitializeRocks()
        {
            List<Rock> rocks = new List<Rock>();

            Rock dash = new Rock(new Vector2Int(4, 1), new Vector2Int[]
            {
                new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0)
            });
            rocks.Add(dash);

            Rock plus = new Rock(new Vector2Int(3, 3), new Vector2Int[]
            {
                new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(1, 2)
            });
            rocks.Add(plus);

            Rock el = new Rock(new Vector2Int(3, 3), new Vector2Int[]
            {
                new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2)
            });
            rocks.Add(el);

            Rock pillar = new Rock(new Vector2Int(1, 4), new Vector2Int[]
            {
                new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(0, 3)
            });
            rocks.Add(pillar);

            Rock box = new Rock(new Vector2Int(2, 2), new Vector2Int[]
            {
                new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1)
            });
            rocks.Add(box);

            return rocks.ToArray();
        }
    }

    internal class Rock
    {
        public Vector2Int[] Units;
        public Vector2Int Bounds;

        public Rock(Vector2Int bounds, Vector2Int[] units)
        {
            Units = units;
            Bounds = bounds;
        }

        public Rock Copy()
        {
            return new Rock(Bounds, Units);
        }
    }
}
