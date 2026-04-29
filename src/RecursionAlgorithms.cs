using System;

public class RecursionOne
{
    private static double F(int i, int n, double x, ref double sum)
    {
        double fi = (i == 1) ? 1.0 : 
        F(i - 1, n, x, ref sum) * (x * x) / (4.0 * (i - 1) * (i - 1) - 2.0 * (i - 1));
        sum += fi;
        return fi;
    }

    public static double Compute(int n, double x)
    {
        if (n <= 0)
            throw new ArgumentOutOfRangeException(nameof(n), "Value of n must be positive");
        if (Math.Abs(x) >= 1_000_000)
            throw new ArgumentOutOfRangeException(nameof(x), "x must satisfy |x| < 10^6");

        double sum = 0;
        F(n, n, x, ref sum);
        return sum;
    }
}

public class RecursionTwo
{
    private static (double fi, double sum) F(int i, double x)
    {
        if (i == 1)
            return (1.0, 1.0);

        var (prevFi, prevSum) = F(i - 1, x);
        double fi = prevFi * (x * x) / (4.0 * (i - 1) * (i - 1) - 2.0 * (i - 1));
        return (fi, prevSum + fi);
    }

    public static double Compute(int n, double x)
    {
        if (n <= 0)
            throw new ArgumentOutOfRangeException(nameof(n), "Value of n must be positive");
        if (Math.Abs(x) >= 1_000_000)
            throw new ArgumentOutOfRangeException(nameof(x), "x must satisfy |x| < 10^6");

        var (_, sum) = F(n, x);
        return sum;
    }
}

public class RecursionThree
{
    private static double F(int i, double x, out double sum)
    {
        if (i == 1)
        {
            sum = 1.0;
            return 1.0;
        }

        double prevFi = F(i - 1, x, out double prevSum);
        double fi = prevFi * (x * x) / (4.0 * (i - 1) * (i - 1) - 2.0 * (i - 1));
        sum = prevSum + fi;
        return fi;
    }

    public static double Compute(int n, double x)
    {
        if (n <= 0)
            throw new ArgumentOutOfRangeException(nameof(n), "Value of n must be positive");
        if (Math.Abs(x) >= 1_000_000)
            throw new ArgumentOutOfRangeException(nameof(x), "x must satisfy |x| < 10^6");

        F(n, x, out double sum);
        return sum;
    }
}
