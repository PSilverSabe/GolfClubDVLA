using GolfClubDVLA.Models;
using Xunit;

namespace GolfClubDVLA.Tests;

/// <summary>
/// Tests the validation and construction rules enforced by the Member record.
/// </summary>
public class MemberTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithNonPositiveNumber_ThrowsArgumentOutOfRangeException(int number)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Member(number, "Jim Parr", 10));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithNonPositiveNumberForPar_Success(int number)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Member(number, "Jim Parr", 10));

        Member member = new(1, "Jim Parr", number);
        Assert.True(member.Handicap == 0 || member.Handicap == -1);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithEmptyOrWhitespaceName_ThrowsArgumentException(string? name)
    {
        Assert.Throws<ArgumentException>(() => new Member(1, name!, 10));
    }

    [Fact]
    public void Constructor_TrimsWhitespaceFromName()
    {
        Member member = new(1, "  Jim Parr  ", 10);

        Assert.Equal("Jim Parr", member.Name);
    }

    [Fact]
    public void Constructor_WithValidValues_SetsAllProperties()
    {
        Member member = new(7, "Jon Rahm", 4);

        Assert.Equal(7, member.Number);
        Assert.Equal("Jon Rahm", member.Name);
        Assert.Equal(4, member.Handicap);
    }
}
