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
            CheckForSwitch(x, 10, ref p);
            int y = p == true ? 1 : -1;

            list.AddLast(y);
        }

        return list;
    }

    public static void ReArrange(LinkedList<int> list, bool p = true)
    {
        LinkedListNode<int> node = list.First;
        LinkedListNode<int> nextNode = node.Next;

        LinkedList<int> savedNodes = new LinkedList<int>();

        int i = 1;
        int steps = 5;

        while (node != null)
        {
            CheckForSwitch(i, steps, ref p);

            if (p == false)
            {
                LinkedListNode<int> save = node;
                list.Remove(node);
                savedNodes.AddLast(save);
            }
            if (p == true && i > steps)
            {
                // now we want to take the first node from savedNodes and 
                // add it after the current node
                // than we want to delete this node from savedNodes, so that we can sample the 
                // next node in the same way
                list.AddAfter(node, savedNodes.First);
                savedNodes.Remove(savedNodes.First);
            }

            Console.WriteLine($"element{i}: {node}");
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