using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day9
    {
        public static List<Vector2> visited = new List<Vector2>() { new Vector2(0, 0) };

        public static int SimulateRope(string input, int ropeLength)
        {
            RopeSegment rope = new RopeSegment(ropeLength);

            foreach (string line in input.Split('\n'))
            {
                string[] inp = line.Split(' ');
                for (int i = 0; i < Convert.ToInt32(inp[1]); i++)
                {
                    if (inp[0] == "U")
                        rope.MoveSegment(RopeSegment.Direction.Up);
                    if (inp[0] == "D")
                        rope.MoveSegment(RopeSegment.Direction.Down);
                    if (inp[0] == "L")
                        rope.MoveSegment(RopeSegment.Direction.Left);
                    if (inp[0] == "R")
                        rope.MoveSegment(RopeSegment.Direction.Right);
                }
            }

            return visited.Count;
        }
    }

    internal class RopeSegment
    {
        public Vector2 pos = new Vector2();
        public RopeSegment? child;
        public int index = 0;

        public RopeSegment(int segments, int index = 0)
        {
            Console.WriteLine($"Creating rope segment [index {index}]");
            this.index = index;
            pos = new Vector2(0, 0);
            if (index+1 < segments)
            {
                child = new RopeSegment(segments, this.index+1);
            }
        }

        public void MoveSegment(Direction direction)
        {
            Vector2 oldPos = pos;
            if (direction == Direction.Up)
                pos += new Vector2(0, 1);
            if (direction == Direction.Down)
                pos += new Vector2(0, -1);
            if (direction == Direction.Left)
                pos += new Vector2(-1, 0);
            if (direction == Direction.Right)
                pos += new Vector2(1, 0);

            //if (CheckDistance())
            //    child.SetPos(oldPos);

            MoveChild();
        }

        public void MoveChild()
        {
            if (child == null)
            {
                if (!Day9.visited.Contains(pos))
                    Day9.visited.Add(pos);
                return;
            }

            Vector2 diff = pos - child.pos;
            
            if (!CheckDistance())
            {
                child.MoveChild();
                return;
            }

            if (diff.X < 0)
            {
                child.pos += new Vector2(-1, 0);
            }
            if (diff.X > 0)
            {
                child.pos += new Vector2(1, 0);
            }
            if (diff.Y < 0)
            {
                child.pos += new Vector2(0, -1);
            }
            if (diff.Y > 0)
            {
                child.pos += new Vector2(0, 1);
            }

            child.MoveChild();
        }

        public void SetPos(Vector2 newPos)
        {
            Vector2 oldPos = pos;
            pos = newPos;

            if (child == null)
            {
                if (!Day9.visited.Contains(pos))
                    Day9.visited.Add(pos);
                return;
            }

            if (CheckDistance())
                child.SetPos(oldPos);
        }

        private bool CheckDistance()
        {
            if (child == null)
                return false;
            if (Math.Abs(pos.X - child.pos.X) > 1)
                return true;
            if (Math.Abs(pos.Y - child.pos.Y) > 1)
                return true;
            return false;
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
