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
        public static int SimulateRope(string input, int ropeLength)
        {
            new RopeSegment(2);
            return 0;
        }
    }

    internal class RopeSegment
    {
        public Vector2 pos = new Vector2();
        public RopeSegment child;
        public int index = 0;

        public RopeSegment(int segments, int index = 0)
        {
            Console.WriteLine($"Creating rope segment [index {index}]");
            this.index = index;
            pos = new Vector2(0, 0);
            if (index++ < segments)
            {
                child = new RopeSegment(segments, this.index++);
            }
        }
    }
}
