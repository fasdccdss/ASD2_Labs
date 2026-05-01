using System;
using System.Collections.Generic;

public class CollectionChains
{
    private LinkedList<int> BuildDictionary(int n, bool p = true)
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

            int y = ListValue(x, 10, ref p);

            list.AddLast(y);
        }

        return list;
    }

    private int ListValue(int x, int y, ref bool p)
    {
        CheckForSwitch(x, y, ref p);

        int i = p == true ? 1 : -1;
        return i;
    }
    private void CheckForSwitch(int x, int y, ref bool p)
    {
        if (x % y == 0)
        {
            p = SwitchBool(p);
        }
    }
    private bool SwitchBool(bool x)
    {
        return !x;
    }

    private LinkedList<int> Rearrange(LinkedList<int> list, bool p = true)
    {
        LinkedListNode<int> node = list.First;
        int i = 1;

        while (node != null)
        {
            CheckForSwitch(i, 5, ref p);
            int y = p == true ? 1 : -1;



            i++;
            node = node.Next;
        }

        return list;
    }
}