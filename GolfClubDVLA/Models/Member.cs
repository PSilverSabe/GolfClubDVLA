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

        Number = number;
        Name = name.Trim();
        Handicap = handicap;
    }
}
