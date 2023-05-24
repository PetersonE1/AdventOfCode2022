using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
    internal static class Day11
    {
        internal static bool doDivideStress = true;

        public static int MonkeyBusiness(string input, int rounds, bool divideStress)
        {
            doDivideStress = divideStress;
            List<Monkey> monkeys = new List<Monkey>();
            foreach (string s in input.Split("\r\n\r\n"))
                monkeys.Add(ParseMonkey(s));

            for (int i = 0; i < rounds; i++)
            {
                foreach (Monkey monkey in monkeys)
                    monkey.InspectItems(monkeys);
            }

            int index = 0;
            List<int> itemsInspected = new List<int>();
            foreach (Monkey monkey in monkeys)
            {
                itemsInspected.Add(monkey.itemsInspected);
                Console.WriteLine($"Monkey {index} inspected {monkey.itemsInspected} items");
                index++;
            }

            itemsInspected.Sort();
            return itemsInspected[^1] * itemsInspected[^2];
        }

        private static Monkey ParseMonkey(string input)
        {
            string[] lines = input.Split('\n');
            List<Item> items = new List<Item>();
            int testNum;
            int operationNum;
            OperationType type = OperationType.Add;
            int trueIndex;
            int falseIndex;

            string itemList = lines[1];
            itemList = itemList.Replace("  Starting items: ", string.Empty);
            foreach (string s in itemList.Split(", "))
            {
                Item item = new Item(Convert.ToInt64(s));
                items.Add(item);
            }

            string operation = lines[2];
            if (operation.Contains('+'))
                type = OperationType.Add;
            if (operation.Contains('*'))
                type = OperationType.Multiply;

            string opNum = operation.Split(' ').Last();
            if (opNum.Contains("old"))
                operationNum = -1;
            else
                operationNum = Convert.ToInt32(opNum);

            testNum = Convert.ToInt32(lines[3].Split(' ').Last());

            trueIndex = Convert.ToInt32(lines[4].Split(' ').Last());
            falseIndex = Convert.ToInt32(lines[5].Split(' ').Last());

            Monkey monkey = new Monkey(items, testNum, operationNum, type, trueIndex, falseIndex);
            return monkey;
        }
    }

    internal class Item
    {
        private static readonly long TRILLION = 1000000000000;
        public long worry;
        public long counter;

        public Item(long worry)
        {
            this.worry = worry;
        }

        public static Item operator +(Item a, long b) => a.AddValue(b);
        public static Item operator -(Item a, long b) => a.SubtractValue(b);
        public static Item operator *(Item a, long b) => a.MultiplyValue(b);
        public static Item operator /(Item a, long b) => a.DivideValue(b);

        public Item AddValue(long val)
        {
            worry += val;
            while (worry > TRILLION)
            {
                counter++;
                worry -= TRILLION;
            }
            return this;
        }

        public Item SubtractValue(long val)
        {
            worry -= val;
            while (worry < 0 && counter > 0)
            {
                worry += TRILLION;
                counter--;
            }
            return this;
        }

        public Item MultiplyValue(long val)
        {
            worry *= val;
            while (worry > TRILLION)
            {
                counter++;
                worry -= TRILLION;
            }
            counter *= val;
            return this;
        }

        public Item DivideValue(long val)
        {
            decimal overflow = ((decimal)counter / val) - (counter / val);
            decimal tempWorry = overflow * TRILLION + worry;
            worry = (long)(tempWorry / val);
            counter /= val;
            while (worry > TRILLION)
            {
                counter++;
                worry -= TRILLION;
            }
            return this;
        }

        public override string ToString()
        {
            return $"{worry}, {counter}";
        }
    }

    internal class Monkey
    {
        public List<Item> items = new List<Item>();
        public int testNumber;
        public int operationNumber;
        public OperationType type;
        public int trueIndex;
        public int falseIndex;

        public int itemsInspected = 0;

        public Monkey(List<Item> items, int testNumber, int operationNumber, OperationType type, int trueIndex, int falseIndex)
        {
            this.items = items;
            this.testNumber = testNumber;
            this.operationNumber = operationNumber;
            this.type = type;
            this.trueIndex = trueIndex;
            this.falseIndex = falseIndex;
        }

        public void InspectItems(List<Monkey> monkeys)
        {
            while (items.Count > 0)
            {
                Item item = items.First();
                long num = (operationNumber == -1) ? item.worry : (uint)operationNumber;
                if (type == OperationType.Add)
                    item += num;
                if (type == OperationType.Multiply)
                    item *= num;
                if (Day11.doDivideStress)
                    item /= 3;

                if ((decimal)item.worry % testNumber == 0)
                {
                    monkeys[trueIndex].items.Add(item);
                    items.RemoveAt(0);
                }
                else
                {
                    monkeys[falseIndex].items.Add(item);
                    items.RemoveAt(0);
                }


                itemsInspected++;
            }
        }
    }

    internal enum OperationType
    {
        Add,
        Multiply
    }
}
