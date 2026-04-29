using System;
using System.Security.Cryptography.X509Certificates;

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
        // guards
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Value of i must be positive");
        }
        if (Math.Abs(x) >= 1_000_000)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "x must satisfy |x| < 10^6");
        }

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
        return F(n, x);
    }
}

public class RecursionThree()
{
    // in this implementation we would first go down the order, than go up and calculate the sum
    public static double F(int n, double x, double[] values, double sum, bool initilized = false)
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
        // operation
        if (!initilized)
        {
            values[0] = 1;
            sum = 0;
            initilized = true;
        }
        if (n != 1)
        {
            values[n] = 
            F(n - 1, x, values, sum, initilized) * (x * x) / (4 * (n - 1) * (n - 1) - 2 * (n - 1));

            Console.WriteLine($"F({n}); value: {values[n]}");
            return values[n];
        }
        else if (n < values.Length)
        {
            int z = n;
            sum += values[z - 1];
            Console.WriteLine($"n = {n}; sum = {sum}");
            return F(n++, x, values, sum, initilized) * (x * x) / (4 * (n - 1) * (n - 1) - 2 * (n - 1));
        }
        Console.WriteLine($"n = {n}; sum = {sum}");
        return sum;
    }
}