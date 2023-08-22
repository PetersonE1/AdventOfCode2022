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
            List<Rock> PlacedRocks = new List<Rock>();
            Rock[] RockTypes = InitializeRocks();

            Rock? heighestRock = null;
            for (int i = 0; i <= 2022; i++)
            {
                int rockIndex = i;
                while (rockIndex >= RockTypes.Length)
                    rockIndex -= RockTypes.Length;

                bool landed = false;
                Rock activeRock = RockTypes[rockIndex].Copy();
                PlacedRocks.Add(activeRock);

                Vector2Int start = heighestRock != null ? 
                    new Vector2Int(1, heighestRock.Position.y + heighestRock.Bounds.y + 3) : new Vector2Int(1, 2);

                int jetIndex = 0;
                while (!landed)
                {
                    while (jetIndex >= input.Length)
                        jetIndex -= input.Length;

                    Vector2Int jetPush = new Vector2Int(-1, 0);
                    if (input[jetIndex] == '>')
                        jetPush = new Vector2Int(1, 0);
                    jetIndex++;

                    Vector2Int target = activeRock.TryShift(jetPush);
                    if (target.x != 7 && target.x != 0 /*&& TODO check other rocks*/)
                    {
                        activeRock.Shift(jetPush);
                    }
                    if (target.y != 0 /*&& TODO check other rocks*/)
                    {
                        activeRock.Shift(new Vector2Int(0, -1));
                        continue;
                    }

                    if (heighestRock == null)
                        heighestRock = activeRock;
                    landed = true;
                    break;
                }

                landed = false;
                if (activeRock.Position.y + activeRock.Bounds.y > heighestRock.Position.y + heighestRock.Bounds.y)
                    heighestRock = activeRock;
            }

            return heighestRock.Position.y + heighestRock.Bounds.y;
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
        public Vector2Int Position = new Vector2Int(0, 0);

        public Rock(Vector2Int bounds, Vector2Int[] units)
        {
            Units = units;
            Bounds = bounds;
        }

        public void SetPosition(Vector2Int position)
        {
            Position = position;
        }

        public void Shift(Vector2Int offset)
        {
            Position += offset;
        }

        public Vector2Int TryShift(Vector2Int offset)
        {
            return Position + offset;
        }

        public bool CheckCollision(Rock other)
        {
            Range thisx = new Range(Position.x, Position.x + Bounds.x);
            Range otherx = new Range(other.Position.x, other.Position.x + other.Bounds.x);
            Range thisy = new Range(Position.y, Position.y + Bounds.y);
            Range othery = new Range(other.Position.y, other.Position.y + other.Bounds.y);
            if (!thisx.Overlaps(otherx) && !thisy.Overlaps(othery))
                return false;

            // TODO: check collision of individual units; filter units to those inside overlapping bounds

            // DEBUG
            return true;
        }

        public Rock Copy()
        {
            return new Rock(Bounds, Units);
        }
    }
}
