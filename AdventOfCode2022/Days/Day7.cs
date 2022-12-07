using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day7
    {
        private static Dictionary<string, Directory> Directories = new Dictionary<string, Directory>();
        private static string currentDirectory = "/";

        public static int DirectorySum(string input, bool firstDay)
        {
            string[] s_input = input.Split("\r\n");

            Directory root = new Directory("/") { Path = "/"};
            Directories.Add(root.Path, root);

            foreach (string s in s_input)
            {
                string[] line = s.Split(' ');
                string[] args = line.CloneExcluding(new int[] { 0 });
                switch (line[0])
                {
                    case "$": ParseCommand(args); break;
                    case "dir":
                        {
                            Directory dir = new Directory(args[0]);
                            dir.Path = currentDirectory + dir.Name + "/";
                            Directories.Add(dir.Path, dir);
                            Directories[currentDirectory].SubDirectories.Add(dir);
                        } break;
                    default:
                        {
                            Directories[currentDirectory].Files.Add(line[1], Convert.ToInt32(line[0]));
                        } break;
                }
            }

            root.GetSize();
            if (firstDay)
            {
                int sum = 0;
                foreach (Directory dir in Directories.Values)
                {
                    if (dir.Size <= 100000)
                        sum += dir.Size;
                }
                return sum;
            }
            else
            {
                return DeleteDirectory();
            }
        }

        private static int DeleteDirectory()
        {
            int _capacity = 70000000;
            int _free = _capacity - Directories["/"].Size;
            int _required = 30000000 - _free;

            List<int> potentials = new List<int>();
            foreach (Directory dir in Directories.Values)
            {
                if (dir.Size >= _required)
                    potentials.Add(dir.Size);
            }
            return potentials.Min();
        }

        private static void ParseCommand(string[] args)
        {
            if (args[0] == "cd")
            {
                switch (args[1])
                {
                    case "/": currentDirectory = "/"; break;
                    case "..":
                        for (int i = currentDirectory.Length - 2; i >= 0; i--)
                        {
                            if (currentDirectory[i] == '/')
                            {
                                currentDirectory = currentDirectory.Remove(i + 1);
                                break;
                            }
                        } break;
                    default: currentDirectory += args[1] + "/"; break;
                }
            }
        }
    }

    internal class Directory
    {
        public string Name;
        public string Path;
        public int Size = -1;

        public List<Directory> SubDirectories = new List<Directory>();
        public Dictionary<string, int> Files = new Dictionary<string, int>();

        public Directory(string directoryName)
        {
            this.Name = directoryName;
        }

        public int GetSize()
        {
            if (Size != -1) return Size;

            int sum = 0;
            foreach (Directory dir in SubDirectories)
            {
                sum += dir.GetSize();
            }
            foreach (var pair in Files)
            {
                sum += pair.Value;
            }
            Size = sum;
            return sum;
        }
    }
}
