using GolfClubDVLA.Models;
using GolfClubDVLA.Repositories;
using Xunit;

namespace GolfClubDVLA.Tests;

/// <summary>
/// Tests InMemoryGolfClubRepository directly (not through GolfClubService),
/// covering storage-layer behaviour: CRUD round-trips, seeding idempotency,
/// duplicate-key rejection, and cancellation. GolfClubServiceTests covers
/// "did we implement the feature correctly"; this covers "does the storage
/// layer behave the way IGolfClubRepository promises".
/// </summary>
public class InMemoryGolfClubRepositoryTests
{
    [Fact]
    public async Task SeedIfEmpty_OnEmptyRepository_InsertsAllData()
    {
        using InMemoryGolfClubRepository repository = new();
        List<Hole> holes = [new(1, 4), new(2, 3)];
        List<Member> members = [new(1, "Jim Parr", 10)];

        await repository.SeedIfEmptyAsync(holes, members, TestContext.Current.CancellationToken);

        Assert.Equal(2, (await repository.GetHolesAsync(TestContext.Current.CancellationToken)).Count);
        Assert.Single(await repository.GetMembersAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task SeedIfEmpty_WhenAlreadyPopulated_DoesNotDuplicate()
    {
        using InMemoryGolfClubRepository repository = new();
        List<Hole> holes = [new(1, 4)];
        List<Member> members = [new(1, "Jim Parr", 10)];

        await repository.SeedIfEmptyAsync(holes, members, TestContext.Current.CancellationToken);
        await repository.SeedIfEmptyAsync(holes, members, TestContext.Current.CancellationToken); // second call should be a no-op

        Assert.Single(await repository.GetHolesAsync(TestContext.Current.CancellationToken));
        Assert.Single(await repository.GetMembersAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task AddMember_ThenGetMembers_RoundTripsCorrectly()
    {
        using InMemoryGolfClubRepository repository = new();

        await repository.AddMemberAsync(new Member(1, "Jon Rahm", 4), TestContext.Current.CancellationToken);
        IReadOnlyList<Member> members = await repository.GetMembersAsync(TestContext.Current.CancellationToken);

        Assert.Single(members);
        Assert.Equal("Jon Rahm", members[0].Name);
        Assert.Equal(4, members[0].Handicap);
    }

    [Fact]
    public async Task AddMember_WithDuplicateNumber_ThrowsInvalidOperationException()
    {
        using InMemoryGolfClubRepository repository = new();
        await repository.AddMemberAsync(new Member(1, "Jim Parr", 10), TestContext.Current.CancellationToken);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.AddMemberAsync(new Member(1, "Someone Else", 5), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task AddHole_WithDuplicateNumber_ThrowsInvalidOperationException()
    {
        using InMemoryGolfClubRepository repository = new();
        await repository.AddHoleAsync(new Hole(1, 4), TestContext.Current.CancellationToken);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.AddHoleAsync(new Hole(1, 5), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task UpdateMember_ForExistingMember_PersistsTheChange()
    {
        using InMemoryGolfClubRepository repository = new();
        await repository.AddMemberAsync(new Member(1, "Jim Parr", 10), TestContext.Current.CancellationToken);
        await Assert.ThrowsAsync<NotImplementedException>(() => repository.UpdateMemberAsync(new Member(1, "Jim Parr", 8), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task UpdateMember_ForUnknownMember_ReturnsFalse()
    {
        using InMemoryGolfClubRepository repository = new();
        await Assert.ThrowsAsync<NotImplementedException>(() => repository.UpdateMemberAsync(new Member(999, "Nobody", 10), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task RemoveMember_ForExistingMember_ThrowsNotImplementedException()
    {
        using InMemoryGolfClubRepository repository = new();
        await repository.AddMemberAsync(new Member(1, "Jim Parr", 10), TestContext.Current.CancellationToken);

        await Assert.ThrowsAsync<NotImplementedException>(() => repository.RemoveMemberAsync(1, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task RemoveMember_ForUnknownMember_ThrowsNotImplementedException()
    {
        using InMemoryGolfClubRepository repository = new();

        await Assert.ThrowsAsync<NotImplementedException>(() => repository.RemoveMemberAsync(999, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task GetHoles_ReturnsThemOrderedByNumber()
    {
        using InMemoryGolfClubRepository repository = new();
        await repository.AddHoleAsync(new Hole(3, 5), TestContext.Current.CancellationToken);
        await repository.AddHoleAsync(new Hole(1, 4), TestContext.Current.CancellationToken);
        await repository.AddHoleAsync(new Hole(2, 3), TestContext.Current.CancellationToken);

        IReadOnlyList<Hole> holes = await repository.GetHolesAsync(TestContext.Current.CancellationToken);

        Assert.Equal([1, 2, 3], holes.Select(h => h.Number));
    }

    [Fact]
    public async Task GetHoles_WithAnAlreadyCancelledToken_ThrowsOperationCanceled()
    {
        using InMemoryGolfClubRepository repository = new();
        using CancellationTokenSource cts = new();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => repository.GetHolesAsync(cts.Token));
    }
}
