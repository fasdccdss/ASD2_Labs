using System;
using System.IO.Compression;
using System.IO.Pipelines;
using System.Runtime;
using System.Runtime.CompilerServices;

public class RecursionOne
{
    public static double F(int n, double x, double sum = 0) 
    {
        // guards
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Value of n must be positive");
        }
        if (Math.Abs(x) >= 1_000_000)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "x must satisfy |x| < 10^6");
        }

        double result = 0;

        if (n == 1) 
        {
            result = 1;
            sum += result;
            Console.WriteLine($"F({n}) Result: {result}");
            Console.WriteLine($"F({n}) Sum: {sum}");
            return result;
        }
        // operation
        result = F(n - 1, x, sum) * (x * x) / (4 * (n - 1) * (n - 1) - 2 * (n - 1));
        sum += result;
        Console.WriteLine($"F({n}) Result: {result}");
        return result;
    }
}

public class RecursionTwo()
{
    private static double F(int n, double x, int i = 1, double result = 1, double sum = 1)
    {
        if (i == n)
        {
            Console.WriteLine($"F({i}) Sum: {sum}");
            return result;
        }

        double nextResult = result * (x * x) / (4 * i * i - 2 * i);
        Console.WriteLine($"F({i}) Result: {result}");

        sum += nextResult;

        return F(n, x, i + 1, nextResult, sum);
    }
    public static double Compute(int n, double x)
    {
        // guards
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Value of i must be positive");
        }
        if (Math.Abs(x) >= 1_000_000)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "x must satisfy |x| < 10^6");
        }

        return F(n, x);
    }
}

public class RecursionThree()
{
    private static double F(int n, double x)
    {
        // guards
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Value of n must be positive");
        }
        if (Math.Abs(x) >= 1_000_000)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "x must satisfy |x| < 10^6");
        }

        if (n == 1) return 1;
        // operation
        double result = F(n - 1, x) * (x * x) / (4 * (n - 1) * (n - 1) - 2 * (n - 1));
        Console.WriteLine($"F({n}) Result: {result}");
        return result;
    }
    private static double Sum(int n, double x)
    {
        if (n == 1) return F(1, x);

        return F(n, x) + Sum(n - 1, x);
    }
    public static double Compute(int n, double x)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Value of n must be positive");
        }
        if (Math.Abs(x) >= 1_000_000)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "x must satisfy |x| < 10^6");
        }

        double sum = Sum(n, x);
        Console.WriteLine($"F({n}) Sum: {sum}");
        return sum;
    }
}