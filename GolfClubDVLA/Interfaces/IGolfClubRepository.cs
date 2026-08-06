using GolfClubDVLA.Models;

namespace GolfClubDVLA.Interfaces;

public interface IGolfClubRepository : IDisposable
{
    Task<IReadOnlyList<Hole>> GetHolesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Member>> GetMembersAsync(CancellationToken cancellationToken = default);

    /// <exception cref="InvalidOperationException">A hole with the same number already exists.</exception>
    Task AddHoleAsync(Hole hole, CancellationToken cancellationToken = default);

    /// <exception cref="InvalidOperationException">A member with the same number already exists.</exception>
    Task AddMemberAsync(Member member, CancellationToken cancellationToken = default);

    Task<bool> UpdateMemberAsync(Member member, CancellationToken cancellationToken = default);

    Task<bool> RemoveMemberAsync(int memberNumber, CancellationToken cancellationToken = default);

    Task SeedIfEmptyAsync(IEnumerable<Hole> holes, IEnumerable<Member> members, CancellationToken cancellationToken = default);
}
