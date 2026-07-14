using System;
using System.Threading;

namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        long totalBits = BitConverter.DoubleToInt64Bits(0.0);
        var barrier = new Barrier(threadsNumber);
        double segmentLength = (b - a) / threadsNumber;
        var threads = new Thread[threadsNumber];
        for (int i = 0; i < threadsNumber; i++)
        {
            int threadIndex = i;
            threads[threadIndex] = new Thread(() =>
            {
                double segmentA = a + threadIndex * segmentLength;
                double segmentB = segmentA + segmentLength;
                double localResult = ComputeTrapezoid(segmentA, segmentB, function, step);
                AddDouble(ref totalBits, localResult);
                barrier.SignalAndWait();
            });
            threads[threadIndex].Start();
        }
        for (int i = 0; i < threadsNumber; i++)
        {
            threads[i].Join();
        }

        return BitConverter.Int64BitsToDouble(Interlocked.Read(ref totalBits));
    }

    private static double ComputeTrapezoid(double a, double b, Func<double, double> function, double step)
    {
        double sum = 0.0;
        int steps = (int)Math.Ceiling((b - a) / step);

        if (steps <= 0)
        {
            return 0.0;
        }

        double actualStep = (b - a) / steps;
        double x = a;
        double previousValue = function(x);

        for (int i = 1; i <= steps; i++)
        {
            double nextX = a + i * actualStep;
            double nextValue = function(nextX);
            sum += (previousValue + nextValue) * actualStep / 2.0;
            previousValue = nextValue;
        }

        return sum;
    }

    private static void AddDouble(ref long totalBits, double valueToAdd)
    {
        long initialValue, computedValue;
        do
        {
            initialValue = Interlocked.Read(ref totalBits);
            double currentValue = BitConverter.Int64BitsToDouble(initialValue);
            double newValue = currentValue + valueToAdd;
            computedValue = BitConverter.DoubleToInt64Bits(newValue);
        }
        while (Interlocked.CompareExchange(ref totalBits, computedValue, initialValue) != initialValue);
        // test CI
    }
}
