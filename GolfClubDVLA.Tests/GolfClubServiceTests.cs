using GolfClubDVLA.Models;
using GolfClubDVLA.Repositories;
using GolfClubDVLA.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace GolfClubDVLA.Tests;

/// <summary>
/// Tests GolfClubService against a real InMemoryGolfClubRepository.
///
/// Methods that are currently stubbed with NotImplementedException are still tested here,
/// pinning down today's behaviour.
/// </summary>
public class GolfClubServiceTests
{
    private static GolfClubService CreateService(InMemoryGolfClubRepository repository)
    {
        return new GolfClubService(repository, NullLogger<GolfClubService>.Instance);
    }


    [Fact]
    public async Task TotalParAsync_WithMultipleHoles_ReturnsSumOfPar()
    {
        using InMemoryGolfClubRepository repository = new();
        await repository.AddHoleAsync(new Hole(1, 4), TestContext.Current.CancellationToken);
        await repository.AddHoleAsync(new Hole(2, 3), TestContext.Current.CancellationToken);
        await repository.AddHoleAsync(new Hole(3, 5), TestContext.Current.CancellationToken);
        GolfClubService club = CreateService(repository);

        int totalPar = await club.TotalParAsync(TestContext.Current.CancellationToken);

        Assert.Equal(12, totalPar);
    }

    [Fact]
    public async Task TotalParAsync_WithNoHoles_ReturnsZero()
    {
        using InMemoryGolfClubRepository repository = new();
        GolfClubService club = CreateService(repository);

        int totalPar = await club.TotalParAsync(TestContext.Current.CancellationToken);

        Assert.Equal(0, totalPar);
    }


    [Fact]
    public async Task MembersWithHandicapBelowAsync_ReturnsOnlyMembersStrictlyBelowThreshold()
    {
        using InMemoryGolfClubRepository repository = new();
        await repository.AddMemberAsync(new Member(1, "Jim Parr", 10), TestContext.Current.CancellationToken);
        await repository.AddMemberAsync(new Member(2, "Jon Rahm", 4), TestContext.Current.CancellationToken);
        await repository.AddMemberAsync(new Member(3, "Ernie Elsif", 18), TestContext.Current.CancellationToken);
        GolfClubService club = CreateService(repository);

        IReadOnlyList<Member> result = await club.MembersWithHandicapBelowAsync(11, TestContext.Current.CancellationToken);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, m => m.Number == 1);
        Assert.Contains(result, m => m.Number == 2);
        Assert.DoesNotContain(result, m => m.Number == 3);
    }

    [Fact]
    public async Task MembersWithHandicapBelowAsync_ThresholdEqualToHandicap_ExcludesThatMember()
    {
        using InMemoryGolfClubRepository repository = new();
        await repository.AddMemberAsync(new Member(1, "Jim Parr", 10), TestContext.Current.CancellationToken);
        GolfClubService club = CreateService(repository);

        IReadOnlyList<Member> result = await club.MembersWithHandicapBelowAsync(10, TestContext.Current.CancellationToken);

        Assert.Empty(result);
    }

    [Fact]
    public async Task MembersWithHandicapBelowAsync_WithNoMembers_ReturnsEmptyList()
    {
        using InMemoryGolfClubRepository repository = new();
        GolfClubService club = CreateService(repository);

        IReadOnlyList<Member> result = await club.MembersWithHandicapBelowAsync(54, TestContext.Current.CancellationToken);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AverageHandicapAsync_IsNotYetImplemented()
    {
        using InMemoryGolfClubRepository repository = new();
        GolfClubService club = CreateService(repository);

        await Assert.ThrowsAsync<NotImplementedException>(() => club.AverageHandicapAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task LowestHandicapMemberAsync_IsNotYetImplemented()
    {
        using InMemoryGolfClubRepository repository = new();
        GolfClubService club = CreateService(repository);

        await Assert.ThrowsAsync<NotImplementedException>(() => club.LowestHandicapMemberAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task ParBreakdownAsync_IsNotYetImplemented()
    {
        using InMemoryGolfClubRepository repository = new();
        GolfClubService club = CreateService(repository);

        await Assert.ThrowsAsync<NotImplementedException>(() => club.ParBreakdownAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task FindMemberAsync_IsNotYetImplemented()
    {
        using InMemoryGolfClubRepository repository = new();
        GolfClubService club = CreateService(repository);

        await Assert.ThrowsAsync<NotImplementedException>(() => club.FindMemberAsync(1, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task FindMembersByNameAsync_IsNotYetImplemented()
    {
        using InMemoryGolfClubRepository repository = new();
        GolfClubService club = CreateService(repository);

        await Assert.ThrowsAsync<NotImplementedException>(() => club.FindMembersByNameAsync("Jon Rahm", TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task AddMemberAsync_IsNotYetImplemented()
    {
        using InMemoryGolfClubRepository repository = new();
        GolfClubService club = CreateService(repository);

        await Assert.ThrowsAsync<NotImplementedException>(
            () => club.AddMemberAsync(new Member(5, "Tiger Woods", 0), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task UpdateMemberHandicapAsync_IsNotYetImplemented()
    {
        using InMemoryGolfClubRepository repository = new();
        GolfClubService club = CreateService(repository);

        await Assert.ThrowsAsync<NotImplementedException>(
            () => club.UpdateMemberHandicapAsync(1, 8, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task RemoveMemberAsync_IsNotYetImplemented()
    {
        using InMemoryGolfClubRepository repository = new();
        GolfClubService club = CreateService(repository);

        await Assert.ThrowsAsync<NotImplementedException>(() => club.RemoveMemberAsync(1, TestContext.Current.CancellationToken));
    }
}
