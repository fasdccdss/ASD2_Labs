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

    public static void Rearrange(LinkedList<int> list, bool p = true)
    {
        LinkedListNode<int> node = list.First;

        LinkedList<int> savedNodes = new LinkedList<int>();

        int i = 1;
        int steps = 5;

        while (node != null)
        {
            LinkedListNode<int> nextNode = node.Next;

            if (p == false)
            {
                // 1*:
                // you can't add a node that belonged to another list — even after removing it, 
                // C# still tracks which list it came from and throws this exception. Use .Value
                savedNodes.AddLast(node.Value);
                list.Remove(node);
            }
            else if (p == true && i > steps)
            {
                // now we want to take the first node from savedNodes and 
                // add it after the current node
                // than we want to delete this node from savedNodes, so that we can sample the 
                // next node in the same way
                list.AddAfter(node, savedNodes.First.Value); // same here a in issue 1*
                savedNodes.Remove(savedNodes.First);
            }

            Console.WriteLine($"element {i}: {node.Value}");
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