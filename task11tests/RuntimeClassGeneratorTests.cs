using Xunit;

namespace task11tests;

public class RuntimeClassGeneratorTests
{
    [Fact]
    public void CreateCalculator_ReturnsWorkingInstance()
    {
        var calculator = task11.RuntimeClassGenerator.CreateCalculator();

        Assert.NotNull(calculator);
        Assert.IsAssignableFrom<task11.ICalculator>(calculator);
    }

    [Fact]
    public void Add_ReturnsCorrectSum()
    {
        var calculator = task11.RuntimeClassGenerator.CreateCalculator();

        var result = calculator.Add(3, 5);

        Assert.Equal(8, result);
    }

    [Fact]
    public void Minus_ReturnsCorrectDifference()
    {
        var calculator = task11.RuntimeClassGenerator.CreateCalculator();

        var result = calculator.Minus(10, 4);

        Assert.Equal(6, result);
    }

    [Fact]
    public void Mul_ReturnsCorrectProduct()
    {
        var calculator = task11.RuntimeClassGenerator.CreateCalculator();

        var result = calculator.Mul(6, 7);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Div_ReturnsCorrectQuotient()
    {
        var calculator = task11.RuntimeClassGenerator.CreateCalculator();

        var result = calculator.Div(20, 4);

        Assert.Equal(5, result);
    }

    [Fact]
    public void Div_ByZero_ThrowsDivideByZeroException()
    {
        var calculator = task11.RuntimeClassGenerator.CreateCalculator();

        Assert.Throws<DivideByZeroException>(() => calculator.Div(10, 0));
    }
}
