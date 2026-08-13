namespace GolfClubDVLA.Models;

/// <summary>
/// A single hole on the course
/// </summary>
public sealed record Hole
{
    public int Number { get; }

    public int Par { get; }

    public decimal Distance { get; set; }

    public Hole(int number, int par, decimal distance)
    {
        if (number <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Hole number must be positive.");
        }

        if (par is < 1 or > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(par), "Par must be between 1 and 10.");
        }

        if (distance <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(distance), "Distance cannot be negative.");
        }

        Number = number;
        Par = par;
        Distance = distance;
    }
}
