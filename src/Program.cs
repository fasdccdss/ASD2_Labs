using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
        /*
        LinkedList<int> list = null;

        while (list == null)
        {
            try
            {
                Console.Write("Enter n: ");
                int n = int.Parse(Console.ReadLine());

                list = CollectionChains.BuildList(n);
                Console.WriteLine("початкові дані:");
                CollectionChains.PrintList(list, n);

                CollectionChains.Rearrange(list);
                Console.WriteLine("одержанi дані:");
                CollectionChains.PrintList(list, n);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        */
        /* LAB 3 */
        // Application.Run(new GraphWindow());

        /* LAB 4 */
        // Application.Run(new GraphCharacteristics());

        /* LAB 5 */
        Application.Run(new GraphBypass());
    }

}