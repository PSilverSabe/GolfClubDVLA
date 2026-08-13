using GolfClubDVLA.Models;
using Xunit;

namespace GolfClubDVLA.Tests;

/// <summary>
/// Tests the validation and construction rules enforced by the Hole record.
/// </summary>
public class HoleTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithNonPositiveNumber_ThrowsArgumentOutOfRangeException(int number)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Hole(number, 4, 340));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Constructor_WithParOutsideAllowedRange_ThrowsArgumentOutOfRangeException(int par)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Hole(1, par, 100));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void Constructor_WithPositiveDistance_Succeeds(int distance)
    {
        Hole hole = new Hole(1, 4, distance);
        Assert.Equal(distance, hole.Distance);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void Constructor_WithNegativeDistanceOrZeroDistance_ThrowsArgumentOutOfRangeException(int distance)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Hole(1, 4, distance));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void Constructor_WithParAtBoundary_Succeeds(int par)
    {
        Hole hole = new(1, par, 100);

        Assert.Equal(par, hole.Par);
    }

    [Fact]
    public void Constructor_WithValidValues_SetsAllProperties()
    {
        Hole hole = new(3, 5, 100);

        Assert.Equal(3, hole.Number);
        Assert.Equal(5, hole.Par);
        Assert.Equal(100, hole.Distance);
    }
}
