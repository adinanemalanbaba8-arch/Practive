using System;
using Xunit;

namespace task14tests;

public class DefiniteIntegralTests
{
    [Fact]
    public void Solve_LinearFunctionSymmetricInterval_ReturnsZero()
    {
        Func<double, double> x = value => value;

        var result = task14.DefiniteIntegral.Solve(-1, 1, x, 1e-4, 2);

        Assert.Equal(0, result, 1e-4);
    }

    [Fact]
    public void Solve_SinFunctionSymmetricInterval_ReturnsZero()
    {
        Func<double, double> sin = value => Math.Sin(value);

        var result = task14.DefiniteIntegral.Solve(-1, 1, sin, 1e-5, 8);

        Assert.Equal(0, result, 1e-4);
    }

    [Fact]
    public void Solve_LinearFunctionFromZeroToFive_ReturnsTen()
    {
        Func<double, double> x = value => value;

        var result = task14.DefiniteIntegral.Solve(0, 5, x, 1e-6, 8);

        Assert.Equal(12.5, result, 1e-5);
    }

    [Fact]
    public void Solve_ConstantFunction_ReturnsRectangleArea()
    {
        Func<double, double> constant = value => 2.0;

        var result = task14.DefiniteIntegral.Solve(0, 3, constant, 1e-4, 4);

        Assert.Equal(6, result, 1e-3);
    }
}
