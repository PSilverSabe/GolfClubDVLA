using GolfClubDVLA.Interfaces;
using GolfClubDVLA.Models;

namespace GolfClubDVLA.Repositories;

/// <summary>
/// Implementation of IGolfClubRepository that stores data in memory. Could be replaced with a database-backed implementation in the future
/// </summary>
public sealed class InMemoryGolfClubRepository : IGolfClubRepository
{
    private readonly Dictionary<int, Hole> _holes = [];
    private readonly Dictionary<int, Member> _members = [];

    public Task<IReadOnlyList<Hole>> GetHolesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyList<Hole>>([.. _holes.Values.OrderBy(h => h.Number)]);
    }

    public Task<IReadOnlyList<Member>> GetMembersAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyList<Member>>([.. _members.Values.OrderBy(m => m.Number)]);
    }

    public Task AddHoleAsync(Hole hole, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_holes.TryAdd(hole.Number, hole))
        {
            throw new InvalidOperationException($"A hole numbered {hole.Number} already exists.");
        }

        return Task.CompletedTask;
    }

    public Task AddMemberAsync(Member member, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_members.TryAdd(member.Number, member))
        {
            throw new InvalidOperationException($"A member numbered {member.Number} already exists.");
        }

        return Task.CompletedTask;
    }

    public Task<bool> UpdateMemberAsync(Member member, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("UpdateMemberAsync is not implemented.");
    }

    public Task<bool> RemoveMemberAsync(int memberNumber, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("RemoveMemberAsync is not implemented.");
    }

    public Task SeedIfEmptyAsync(IEnumerable<Hole> holes, IEnumerable<Member> members, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (_holes.Count == 0)
        {
            foreach (Hole hole in holes)
            {
                _holes[hole.Number] = hole;
            }
        }

        if (_members.Count == 0)
        {
            foreach (Member member in members)
            {
                _members[member.Number] = member;
            }
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        // Nothing to release - kept for interface symmetry (IGolfClubRepository
        // is IDisposable so a future persistent implementation can clean up
        // file handles/connections without changing the interface).
    }
}
