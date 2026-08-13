namespace GolfClubDVLA.Models;

/// <summary>
/// A club member
/// </summary>
public sealed record Member
{
    public int Number { get; }

    public string Name { get; }

    public int Handicap { get; }

    public int RecentScore { get; }

    public Member(int number, string name, int handicap, int recentScore)
    {
        if (number <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Member number must be positive.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must not be empty.", nameof(name));
        }

        if (recentScore <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(recentScore), "Score must be a positive number exceeding zero.");
        }

        Number = number;
        Name = name.Trim();
        Handicap = handicap;
        RecentScore = recentScore;
    }
}
