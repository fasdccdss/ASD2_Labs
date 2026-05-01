using System;
using System.Collections.Generic;

public class CollectionChains
{
    public static LinkedList<int> BuildList(int n, bool p = true)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Value of n must be positive");
        }
        else if (n % 20 != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Value of n must be divisible by 20");
        }

        LinkedList<int> list = new LinkedList<int>();

        for(int i = 0; i < n; i++)
        {
            int x = i + 1;
            int y = p == true ? 1 : -1;

            list.AddLast(y);

            CheckForSwitch(x, 10, ref p);
        }

        return list;
    }

    public static void Rearrange(LinkedList<int> list, int steps = 5, bool p = true)
    {
        LinkedListNode<int> node = list.First;

        int i = 1;

        while (node != null)
        {
            Console.WriteLine($"element {i}: {node.Value}");
            LinkedListNode<int> nextNode = node.Next;

            if (p == false)
            {
                // var nodeValue = node.Value;
                LinkedListNode<int> s = node;

                for (int x = 0; x < steps; x++)
                {
                    if (s.Next != null)
                    {
                        s = s.Next;
                    }
                }

                list.Remove(node);
                list.AddAfter(s, node.Value);
            }

            CheckForSwitch(i, steps, ref p);
            i++;
            node = nextNode;
        }
    }

    private static void CheckForSwitch(int x, int y, ref bool p)
    {
        if (x % y == 0)
        {
            p = SwitchBool(p);
        }
    }
    private static bool SwitchBool(bool x)
    {
        return !x;
    }
}