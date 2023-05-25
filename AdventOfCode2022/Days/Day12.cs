using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day12
    {
        public static int FewestSteps(string input)
        {
            Node? startNode = null;
            Node? exitNode = null;

            int a = 0, b = 0;
            Node[][] nodes = input.Split('\n').Select(i => 
            {
                Node[] ns = i.ToCharArray().Select(j =>
                {
                    Node n = ProcessInput(j, a, b, out NodeType type);
                    if (type == NodeType.Start)
                        startNode = n;
                    if (type == NodeType.Exit)
                        exitNode = n;
                    a++;
                    return n;
                }).ToArray();
                a = 0;
                b++;
                return ns;
            }).ToArray();

            List<Node> openNodes = new List<Node>();
            List<Node> closedNodes = new List<Node>();

            if (startNode == null || exitNode == null)
                return 0;

            Vector2 currentNode = startNode.pos;

            openNodes.Add(startNode);
            return 0;
        }

        private static Node ProcessInput(char input, int x, int y, out NodeType nodeType)
        {
            Vector2 pos = new Vector2(x, y);
            switch (input)
            {
                case 'S': nodeType = NodeType.Start; return new Node(1, pos);
                case 'E': nodeType = NodeType.Exit; return new Node(26, pos);
                default: nodeType = NodeType.None; return new Node((int)input - 96, pos);
            }
        }

        private static T GetElementAt<T>(this T[][] values, Vector2 pos)
        {
            return values[(int)pos.Y][(int)pos.X];
        }
    }

    internal class Node
    {
        public double g = 0;
        public double h = 0;
        public double f => g + h;
        public int height = 0;
        public Vector2 pos;

        public List<Node> children = new List<Node>();

        public Node(int height, Vector2 pos)
        {
            this.height = height;
            this.pos = pos;
        }
    }

    internal enum NodeType
    {
        None,
        Start,
        Exit
    }
}
