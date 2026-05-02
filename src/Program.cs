using System;
using System.Diagnostics;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        /* LAB 1 */
        /*
        Console.WriteLine("Програма 1:");
        RecursionOne.Compute(5, 10);
        Console.WriteLine("Програма 2:");
        RecursionTwo.Compute(5, 5);
        Console.WriteLine("Програма 3:");
        RecursionThree.Compute(5, 4.67f);
        */

        /* LAB 2 */
        LinkedList<int> list = CollectionChains.BuildList(40);
        CollectionChains.Rearrange(list);
        CollectionChains.PrintList(list);
    }
}