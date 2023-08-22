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
                    new Vector2Int(2, heighestRock.Position.y + heighestRock.Bounds.y + 3) : new Vector2Int(2, 3);
                activeRock.Position = start;

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
                    target.y--;
                    bool collided = false;
                    bool settled = false;

                    foreach (Rock other in PlacedRocks)
                    {
                        if (other == activeRock) continue;
                        if (activeRock.CheckCollision(other, jetPush))
                            collided = true;
                        if (activeRock.CheckCollision(other, new Vector2Int(0, -1)))
                            settled = true;
                        if (collided && settled)
                            break;
                    }

                    if (target.x != 8 && target.x != 0 && !collided)
                    {
                        activeRock.Shift(jetPush);
                    }
                    if (target.y != 0 && !settled)
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
                if (activeRock.Position.y + activeRock.Bounds.y - 1 > heighestRock.Position.y + heighestRock.Bounds.y - 1)
                    heighestRock = activeRock;
                //Console.WriteLine(heighestRock.Position.y + heighestRock.Bounds.y - 1);
                DisplayGrid(PlacedRocks);
                Console.Read();
            }

            return heighestRock.Position.y + heighestRock.Bounds.y - 1;
        }

        private static void DisplayGrid(List<Rock> rocks)
        {
            int width = 7;
            int height = 50;

            for (int i = height; i > 0; i--)
            {
                for (int j = 0; j < width; j++)
                {
                    bool broken = false;
                    Vector2Int v = new Vector2Int(j, i);
                    foreach (Rock rock in rocks)
                    {
                        foreach (Vector2Int unit in rock.Units)
                        {
                            if (unit + rock.Position == v)
                            {
                                Console.Write('#');
                                broken = true;
                                break;
                            }
                        }
                        if (broken) break;
                    }
                    if (!broken)
                        Console.Write('.');
                }
                Console.Write('\n');
            }
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
            if (!thisx.Overlaps(otherx) || !thisy.Overlaps(othery))
                return false;

            Range overlap_x = thisx.GetOverlap(otherx);
            Range overlap_y = thisy.GetOverlap(othery);

            Range check_thisx = new Range(overlap_x.Start.Value - Position.x, overlap_x.End.Value - Position.x);
            Range check_thisy = new Range(overlap_y.Start.Value - Position.y, overlap_y.End.Value - Position.y);
            Range check_otherx = new Range(overlap_x.Start.Value - other.Position.x, overlap_x.End.Value - other.Position.x);
            Range check_othery = new Range(overlap_y.Start.Value - other.Position.y, overlap_y.End.Value - other.Position.y);

            Vector2Int[] thisUnits = Units.Select(k => k += Position).Where(u => (u.x >= check_thisx.Start.Value && u.x <= check_thisx.End.Value)
            && (u.y >= check_thisy.Start.Value && u.y <= check_thisy.End.Value)).ToArray();

            Vector2Int[] otherUnits = other.Units.Select(k => k += other.Position).Where(u => (u.x >= check_otherx.Start.Value && u.x <= check_otherx.End.Value)
            && (u.y >= check_othery.Start.Value && u.y <= check_othery.End.Value)).ToArray();

            foreach (Vector2Int unit in thisUnits)
                foreach (Vector2Int otherUnit in otherUnits)
                    if (unit.Equals(otherUnit))
                        return true;

            return false;
        }

        public bool CheckCollision(Rock other, Vector2Int offset)
        {
            Vector2Int offsetPosition = this.TryShift(offset);

            Range thisx = new Range(offsetPosition.x, offsetPosition.x + Bounds.x);
            Range otherx = new Range(other.Position.x, other.Position.x + other.Bounds.x);
            Range thisy = new Range(offsetPosition.y, offsetPosition.y + Bounds.y);
            Range othery = new Range(other.Position.y, other.Position.y + other.Bounds.y);
            if (!thisx.Overlaps(otherx) || !thisy.Overlaps(othery))
                return false;

            Range overlap_x = thisx.GetOverlap(otherx);
            Range overlap_y = thisy.GetOverlap(othery);

            Range check_thisx = new Range(overlap_x.Start.Value - offsetPosition.x, overlap_x.End.Value - offsetPosition.x);
            Range check_thisy = new Range(overlap_y.Start.Value - offsetPosition.y, overlap_y.End.Value - offsetPosition.y);
            Range check_otherx = new Range(overlap_x.Start.Value - other.Position.x, overlap_x.End.Value - other.Position.x);
            Range check_othery = new Range(overlap_y.Start.Value - other.Position.y, overlap_y.End.Value - other.Position.y);

            Vector2Int[] thisUnits = Units.Select(k => k += offsetPosition).Where(u => (u.x >= check_thisx.Start.Value && u.x <= check_thisx.End.Value)
            && (u.y >= check_thisy.Start.Value && u.y <= check_thisy.End.Value)).ToArray();

            Vector2Int[] otherUnits = other.Units.Select(k => k += other.Position).Where(u => (u.x >= check_otherx.Start.Value && u.x <= check_otherx.End.Value)
            && (u.y >= check_othery.Start.Value && u.y <= check_othery.End.Value)).ToArray();

            foreach (Vector2Int unit in thisUnits)
                foreach (Vector2Int otherUnit in otherUnits)
                    if (unit.Equals(otherUnit))
                        return true;

            return false;
        }

        public Rock Copy()
        {
            return new Rock(Bounds, Units);
        }
    }
}
