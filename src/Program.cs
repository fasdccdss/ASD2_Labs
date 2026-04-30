using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine("Програма 1:");
        RecursionOne.Compute(5, 10);
        Console.WriteLine("Програма 2:");
        RecursionTwo.Compute(5, 10);
        Console.WriteLine("Програма 3:");
        RecursionThree.Compute(5, 10);
    }
}