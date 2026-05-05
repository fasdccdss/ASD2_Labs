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

    public static void Rearrange(LinkedList<int> list, int steps = 5, 
    int previousSteps = 10)
    {
        LinkedListNode<int> node = list.First;

        int i = 1;

        while (node != null)
        {
            LinkedListNode<int> nextNode = node.Next;

            if (i % (previousSteps * 2) == previousSteps + steps)
            {
                LinkedListNode<int> save = node;
                LinkedListNode<int> prev = save;

                for (int x = 0; x < previousSteps; x++)
                {
                    if (prev.Previous != null)
                        prev = prev.Previous;
                }

                for (int y = 0; y < steps; y++)
                {
                    LinkedListNode<int> toMove = save;
                    save = save.Previous;
                    list.Remove(toMove);
                    LinkedListNode<int> inserted = list.AddAfter(prev, toMove.Value);
                    prev = inserted;
                }
            }

            i++;
            node = nextNode;
        }
    }

    public static void FreeList(LinkedList<int> list) => list.Clear();
    public static void PrintList(LinkedList<int> list, int n = 0)
    {
        if (n > 0)
        {
            Console.WriteLine($"n = {n}");
        }

        LinkedListNode<int> node = list.First;
        int i = 1;
        while (node != null)
        {
            Console.WriteLine($"element {i}: {node.Value}");
            i++;
            node = node.Next;
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