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
        Assert.Throws<ArgumentOutOfRangeException>(() => new Hole(number, 4));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Constructor_WithParOutsideAllowedRange_ThrowsArgumentOutOfRangeException(int par)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Hole(1, par));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void Constructor_WithParAtBoundary_Succeeds(int par)
    {
        Hole hole = new(1, par);

        Assert.Equal(par, hole.Par);
    }

    [Fact]
    public void Constructor_WithValidValues_SetsAllProperties()
    {
        Hole hole = new(3, 5);

        Assert.Equal(3, hole.Number);
        Assert.Equal(5, hole.Par);
    }
}
