using Xunit;

namespace task04tests;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        task04.ISpaceship cruiser = new task04.Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        task04.ISpaceship fighter = new task04.Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(30, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new task04.Fighter();
        var cruiser = new task04.Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Cruiser_ShouldHaveStrongerFirePowerThanFighter()
    {
        var fighter = new task04.Fighter();
        var cruiser = new task04.Cruiser();
        Assert.True(cruiser.FirePower > fighter.FirePower);
    }

    [Fact]
    public void Cruiser_MoveForward_DoesNotThrow()
    {
        task04.ISpaceship cruiser = new task04.Cruiser();
        var exception = Record.Exception(() => cruiser.MoveForward());
        Assert.Null(exception);
    }

    [Fact]
    public void Fighter_Rotate_DoesNotThrow()
    {
        task04.ISpaceship fighter = new task04.Fighter();
        var exception = Record.Exception(() => fighter.Rotate(90));
        Assert.Null(exception);
    }

    [Fact]
    public void Cruiser_Fire_DoesNotThrow()
    {
        task04.ISpaceship cruiser = new task04.Cruiser();
        var exception = Record.Exception(() => cruiser.Fire());
        Assert.Null(exception);
    }
}
