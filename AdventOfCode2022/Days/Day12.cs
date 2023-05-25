using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day12
    {
        public static int FewestSteps(string input, bool startFromS)
        {
            List<Node> startNodes = new List<Node>();
            Node? exitNode = null;

            int a = 0, b = 0;
            Node[][] nodes = input.Split('\n').Select(i => 
            {
                Node[] ns = i.ToCharArray().Select(j =>
                {
                    Node n = ProcessInput(j, a, b, out NodeType type);
                    if (type == NodeType.Start && startFromS)
                        startNodes.Add(n);
                    if (type == NodeType.Exit)
                        exitNode = n;
                    if (!startFromS && n.height == 1)
                        startNodes.Add(n);
                    a++;
                    return n;
                }).ToArray();
                a = 0;
                b++;
                return ns;
            }).ToArray();

            char[,] display = new char[nodes.Length, nodes[0].Length];
            for (int i = 0; i < display.GetLength(0); i++)
            {
                for (int j = 0; j < display.GetLength(1); j++)
                {
                    display[i, j] = '.';
                }
            }

            List<Node> openNodes = new List<Node>();
            List<Node> closedNodes = new List<Node>();

            if (startNodes.Count == 0 || exitNode == null)
                return 0;

            Dictionary<int, string> stepsList = new Dictionary<int, string>();
            foreach (Node startNode in startNodes)
            {
                Node currentNode = startNode;

                openNodes.Add(startNode);

                while (openNodes.Count > 0)
                {
                    openNodes.Sort();
                    currentNode = openNodes.First();
                    openNodes.Remove(currentNode);
                    closedNodes.Add(currentNode);

                    if (currentNode == exitNode)
                    {
                        int steps = 0;
                        display[(int)currentNode.pos.Y, (int)currentNode.pos.X] = 'E';
                        while (currentNode.parent != null)
                        {
                            Vector2 direction = currentNode.pos - currentNode.parent.pos;

                            steps++;
                            currentNode = currentNode.parent;

                            char toReplace = 'O';
                            if (direction.X != 0) toReplace = direction.X < 0 ? '<' : '>';
                            if (direction.Y != 0) toReplace = direction.Y < 0 ? '^' : 'v';
                            display[(int)currentNode.pos.Y, (int)currentNode.pos.X] = toReplace;
                        }

                        string displayString = string.Empty;
                        for (int i = 0; i < display.GetLength(0); i++)
                        {
                            for (int j = 0; j < display.GetLength(1); j++)
                            {
                                displayString += display[i, j];
                            }
                            displayString += '\n';
                        }
                        //Console.WriteLine(displayString);

                        //return steps;
                        stepsList.TryAdd(steps, displayString);
                        break;
                    }

                    currentNode.children.Clear();
                    for (int i = 1; i != 0; i = i == 1 ? -1 : 0)
                    {
                        Node node1 = nodes.GetElementAt(currentNode.pos + new Vector2(i, 0));
                        Node node2 = nodes.GetElementAt(currentNode.pos + new Vector2(0, i));
                        if (node1 != null)
                        {
                            currentNode.children.Add(node1);
                        }
                        if (node2 != null)
                        {
                            currentNode.children.Add(node2);
                        }
                    }

                    foreach (Node child in currentNode.children)
                    {
                        if (closedNodes.Contains(child))
                            continue;

                        if (openNodes.Contains(child) && child.g < currentNode.g + 1)
                            continue;

                        child.parent = currentNode;
                        child.g = currentNode.g + 1;
                        //child.h = Math.Pow(child.pos.X - exitNode.pos.X, 2) + Math.Pow(child.pos.Y - exitNode.pos.Y, 2);
                        child.h = Math.Abs(child.pos.X - exitNode.pos.X) + Math.Abs(child.pos.Y - exitNode.pos.Y);

                        //if (child.height > currentNode.height)
                        //    child.h -= 1 / (child.height - currentNode.height);

                        if (!openNodes.Contains(child) && (child.height - currentNode.height) <= 1)
                            openNodes.Add(child);
                    }
                }
            }
            ImmutableSortedDictionary<int, string> results = stepsList.ToImmutableSortedDictionary();
            Console.WriteLine(results.First().Value);
            Console.WriteLine("Results:");
            foreach (var pair in results)
            {
                Console.WriteLine(pair.Key);
            }
            return results.First().Key;
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
            if (pos.X < 0 || pos.Y < 0)
                return default;
            try
            {
                return values[(int)pos.Y][(int)pos.X];
            }
            catch
            {
                return default;
            }
        }
    }

    internal class Node : IComparable
    {
        public double g = 0;
        public double h = 0;
        public double f => g + h;
        public int height = 0;
        public Vector2 pos;

        public List<Node> children = new List<Node>();
        public Node parent;

        public Node(int height, Vector2 pos)
        {
            this.height = height;
            this.pos = pos;
        }

        int IComparable.CompareTo(object? obj)
        {
            if (obj == null)
                return 1;
            Node node = (Node)obj;
            return (int)(f - node.f);
        }
    }

    internal enum NodeType
    {
        None,
        Start,
        Exit
    }
}
