using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using task14;
using ScottPlot;

namespace task15;

class Program
{
    static void Main(string[] args)
    {
        Func<double, double> sin = x => Math.Sin(x);
        double a = -100;
        double b = 100;
        double trueValue = 0.0;
        double precision = 1e-4;

        double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        double bestStep = steps[0];

        Console.WriteLine("Шаг 3. Определение минимального размера шага для точности 1e-4");
        Console.WriteLine("Функция: sin(x), отрезок: [-100, 100], точное значение интеграла: 0");
        Console.WriteLine();
        foreach (var step in steps)
        {
            double result = DefiniteIntegral.Solve(a, b, sin, step, 4);
            double error = Math.Abs(result - trueValue);
            bool acceptable = error <= precision;

            Console.WriteLine($"Шаг = {step:E0}: результат = {result:F6}, погрешность = {error:E4}, точность достигнута: {acceptable}");

            if (acceptable)
            {
                bestStep = step;
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Выбранный минимальный по точности, но не избыточный шаг: {bestStep:E0}");
        Console.WriteLine();
        int[] threadCounts = { 1, 2, 3, 4, 5, 6, 7, 8, 10, 12, 16 };
        int repeats = 5;
        var results = new List<(int threads, double avgMs)>();

        foreach (var threadCount in threadCounts)
        {
            var times = new List<long>();
            for (int r = 0; r < repeats; r++)
            {
                var sw = Stopwatch.StartNew();
                DefiniteIntegral.Solve(a, b, sin, bestStep, threadCount);
                sw.Stop();
                times.Add(sw.ElapsedMilliseconds);
            }
            double avg = times.Average();
            results.Add((threadCount, avg));
            Console.WriteLine("Potokov = " + threadCount + ": srednee vremya = " + avg.ToString("F2") + " ms");
        }

        var optimal = results.OrderBy(r => r.avgMs).First();
        Console.WriteLine();
        Console.WriteLine("Optimalnoe chislo potokov: " + optimal.threads + ", srednee vremya: " + optimal.avgMs.ToString("F2") + " ms");
        Console.WriteLine();
                var plt = new ScottPlot.Plot();
        double[] xs = results.Select(r => (double)r.threads).ToArray();
        double[] ys = results.Select(r => r.avgMs).ToArray();
        plt.Add.Scatter(xs, ys);
        plt.Title("Vremya vypolneniya Solve v zavisimosti ot chisla potokov");
        plt.XLabel("Kolichestvo potokov");
        plt.YLabel("Vremya (ms)");
        plt.SavePng("performance_chart.png", 800, 600);
        Console.WriteLine("Grafik sokhranen v fayl: " + Path.GetFullPath("performance_chart.png"));
        Console.WriteLine();
        Console.WriteLine("Sravnenie s odnopotochnoy versiey");
        var swSingle = Stopwatch.StartNew();
        double singleResult = 0.0;
        int stepsCount = (int)Math.Ceiling((b - a) / bestStep);
        double actualStep = (b - a) / stepsCount;
        double prevVal = sin(a);
        for (int i = 1; i <= stepsCount; i++)
        {
            double nextX = a + i * actualStep;
            double nextVal = sin(nextX);
            singleResult += (prevVal + nextVal) * actualStep / 2.0;
            prevVal = nextVal;
        }
        swSingle.Stop();
        long singleMs = swSingle.ElapsedMilliseconds;

        Console.WriteLine("Odnopotochnoe vremya: " + singleMs + " ms");
        Console.WriteLine("Mnogopotochnoe vremya (optimalnoe): " + optimal.avgMs.ToString("F2") + " ms");

        double improvement = (singleMs - optimal.avgMs) / singleMs * 100.0;
        Console.WriteLine("Razlichie: " + improvement.ToString("F2") + " %");

        string reportPath = "results.txt";
        var lines = new List<string>();
        lines.Add("Otchet po podboru optimalnykh parametrov integrirovaniya");
        lines.Add("");
        lines.Add("1. Vybranny shag integrirovaniya: " + bestStep.ToString("E0"));
        lines.Add("   (minimalny po tochnosti shag, obespechivayuschy pogreshnost ne bolee 1e-4 dlya integrala sin(x) na [-100, 100])");
        lines.Add("");
        lines.Add("2. Optimalnoe chislo potokov: " + optimal.threads);
        lines.Add("   (chislo potokov, pri kotorom dostigaetsya minimalnoe srednee vremya vypolneniya funktsii Solve)");
        lines.Add("");
        lines.Add("3. Srednee vremya mnogopotochnoy versii (optimalnoe chislo potokov): " + optimal.avgMs.ToString("F2") + " ms");
        lines.Add("4. Vremya odnopotochnoy versii (bez ispolzovaniya potokov voobsche): " + singleMs + " ms");
        lines.Add("5. Uskorenie mnogopotochnoy versii otnositelno odnopotochnoy: " + improvement.ToString("F2") + " %");
        lines.Add("   (polozhitelnoe znachenie oznachaet, chto mnogopotochnaya versiya rabotaet byistree)");

        File.WriteAllLines(reportPath, lines);
        Console.WriteLine();
        Console.WriteLine("Rezultaty zapisany v fayl: " + Path.GetFullPath(reportPath));
    }
}




