using GolfClubDVLA.Interfaces;
using GolfClubDVLA.Models;
using Microsoft.Extensions.Logging;

namespace GolfClubDVLA.Services;

/// <summary>
/// Golf club service, following standard C# conventions and using dependency injection for the repository and logger.
/// </summary>
public sealed class GolfClubService(IGolfClubRepository repository, ILogger<GolfClubService> logger)
{
    private readonly IGolfClubRepository _repository = repository;
    private readonly ILogger<GolfClubService> _logger = logger;

    /// <summary>
    /// The total par for the course
    /// </summary>
    public async Task<int> TotalParAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Calculating total par for the course.");
        IReadOnlyList<Hole> holes = await _repository.GetHolesAsync(cancellationToken);
        int totalPar = holes.Sum(h => h.Par);
        _logger.LogInformation("Total par calculated: {TotalPar}", totalPar);
        return totalPar;
    }

    /// <summary>
    /// Members whose handicap is lower than maxHandicap
    /// </summary>
    public async Task<IReadOnlyList<Member>> MembersWithHandicapBelowAsync(int maxHandicap, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Finding members with handicap below {MaxHandicap}.", maxHandicap);
        IReadOnlyList<Member> members = await _repository.GetMembersAsync(cancellationToken);
        IReadOnlyList<Member> result = [.. members.Where(m => m.Handicap < maxHandicap)];
        _logger.LogInformation("Found {Count} members with handicap below {MaxHandicap}.", result.Count, maxHandicap);
        return result;
    }

    /// <summary>
    /// Gets the total distance of all holes on the course
    /// </summary>
    public async Task<decimal> GetTotalDistanceOfAllHolesAync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Calculating total distance of all holes.");
        IReadOnlyList<Hole> holes = await _repository.GetHolesAsync(cancellationToken);
        decimal totalDistance = holes.Sum(h => h.Distance);
        _logger.LogInformation("Total distance calculated: {TotalDistance}", totalDistance);
        return totalDistance;
    }

    public async Task<decimal> GetAverageOfAllHolesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Calculating average distance of all holes.");
        IReadOnlyList<Hole> holes = await _repository.GetHolesAsync(cancellationToken);
        decimal averageDistance = holes.Average(h => h.Distance);
        _logger.LogInformation("Average distance calculated: {AverageDistance}", averageDistance);

        return averageDistance;
    }

    /// <summary>
    /// Average handicap across all members
    /// </summary>
    public async Task<double> AverageHandicapAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("AverageHandicapAsync is not implemented.");
    }

    /// <summary>
    /// The member with the lowest handicap
    /// </summary>
    public async Task<Member?> LowestHandicapMemberAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("LowestHandicapMemberAsync is not implemented.");
    }

    /// <summary>
    /// How many holes there are at each par value, e.g. {3: 1, 4: 1, 5: 1}.
    /// </summary>
    public async Task<IReadOnlyDictionary<int, int>> ParBreakdownAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("ParBreakdownAsync is not implemented.");
    }

    /// <summary>
    /// Look up a single member by their number.
    /// </summary>
    public async Task<Member?> FindMemberAsync(int memberNumber, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("FindMemberAsync is not implemented.");
    }

    /// <summary>
    /// Find every member with the given name.
    /// </summary>
    public async Task<IReadOnlyList<Member>> FindMembersByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("FindMembersByNameAsync is not implemented.");
    }

    /// <summary>Register a new member, rejecting a duplicate member number.</summary>
    public async Task AddMemberAsync(Member member, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("AddMemberAsync is not implemented.");
    }

    /// <summary>Update a member's recorded handicap (e.g. after a review).</summary>
    public async Task<bool> UpdateMemberHandicapAsync(int memberNumber, int newHandicap, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("UpdateMemberHandicapAsync is not implemented.");
    }

    /// <summary>Remove a member, e.g. on resignation.</summary>
    public async Task<bool> RemoveMemberAsync(int memberNumber, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("RemoveMemberAsync is not implemented.");
    }
}
