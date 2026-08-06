namespace GolfClubDVLA.Models;

/// <summary>
/// A club member
/// </summary>
public sealed record Member
{
    public int Number { get; }

    public string Name { get; }

    public int Handicap { get; }

    public Member(int number, string name, int handicap)
    {
        if (number <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Member number must be positive.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must not be empty.", nameof(name));
        }

        if (handicap is < 0 or > 54)
        {
            throw new ArgumentOutOfRangeException(nameof(handicap), "Handicap must be between 0 and 54 (the standard WHS range).");
        }

        Number = number;
        Name = name.Trim();
        Handicap = handicap;
    }
}
