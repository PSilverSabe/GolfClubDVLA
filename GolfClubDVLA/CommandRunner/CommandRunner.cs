using GolfClubDVLA.Models;
using GolfClubDVLA.Services;
using Microsoft.Extensions.Logging;

namespace GolfClubDVLA.CommandRunner;

/// <summary>
/// Parses command-line arguments and dispatches to GolfClubService.
///
/// Only total-par and members-below are implemented for the demo, but the other commands are stubbed out for future implementation.
/// </summary>
public static class CommandRunner
{
    public static async Task RunAsync(string[] args, GolfClubService club, ILogger logger, CancellationToken cancellationToken)
    {
        if (args.Length == 0)
        {
            await RunDemoAsync(club, cancellationToken);
            return;
        }

        string command = args[0].ToLowerInvariant();

        switch (command)
        {
            case "--total-par":
                Console.WriteLine(await club.TotalParAsync(cancellationToken));
                break;

            case "--members-below":
                RequireArgs(args, 2, "members-below <maxHandicap>");
                PrintMembers(await club.MembersWithHandicapBelowAsync(ParseInt(args[1], "maxHandicap"), cancellationToken));
                break;

            case "--get-total-distance":
                Console.WriteLine(await club.GetTotalDistanceOfAllHolesAync(cancellationToken));
                break;

            case "--get-average-distance":
                Console.WriteLine(await club.GetAverageOfAllHolesAsync(cancellationToken));
                break;

            case "--find-member":
                throw new NotImplementedException("find-member is not implemented.");

            case "--find-member-by-name":
                throw new NotImplementedException("find-member-by-name is not implemented.");

            case "--add-member":
                throw new NotImplementedException("add-member is not implemented.");

            case "--update-handicap":
                throw new NotImplementedException("update-handicap is not implemented.");

            case "--remove-member":
                throw new NotImplementedException("remove-member is not implemented.");

            case "--demo":
                await RunDemoAsync(club, cancellationToken);
                break;

            case "help":
            case "--help":
            case "-h":
                PrintUsage();
                break;

            default:
                logger.LogWarning("Unrecognised command: {Command}", command);
                PrintUsage();
                break;
        }
    }

    /// <summary>
    /// Demos Feature 1 and Feature 2 on the sheet automatically.
    /// </summary>
    private static async Task RunDemoAsync(GolfClubService club, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Total par for the course: {await club.TotalParAsync(cancellationToken)}");

        const int threshold = 11;
        IReadOnlyList<Member> matching = await club.MembersWithHandicapBelowAsync(threshold, cancellationToken);
        Console.WriteLine($"Members with handicap lower than {threshold}: {FormatList(matching)}");
    }

    private static void PrintUsage()
    {
        Console.WriteLine("""
            Usage: dotnet run --project .\GolfClubDVLA\GolfClubDVLA.csproj --<command> [args]

            Commands:
              --total-par                               Feature 1: total par for the course
              --members-below <maxHandicap>             Feature 2: members with handicap < maxHandicap
              --find-member <number>                    look up a member by number
              --find-member-by-name <name>              look up member(s) by name (case-insensitive)
              --add-member <number> <name> <handicap>   register a new member
              --update-handicap <number> <newHandicap>  change a member's handicap
              --remove-member <number>                  remove a member
              --demo                                    run the full walkthrough (also the default with no args)
              --help/help/-h                            show this message
            """);
    }

    private static void PrintMembers(IReadOnlyList<Member> members)
    {
        if (members.Count == 0)
        {
            Console.WriteLine("No matching members.");
            return;
        }

        Console.WriteLine(FormatList(members));
    }

    private static string FormatList(IEnumerable<Member> members)
    {
        return string.Join(", ", members.Select(Format));
    }

    private static string Format(Member member)
    {
        return $"#{member.Number} {member.Name} (handicap {member.Handicap})";
    }

    private static int ParseInt(string value, string argumentName)
    {
        if (!int.TryParse(value, out int result))
        {
            throw new ArgumentException($"Expected a whole number for {argumentName}, got '{value}'.");
        }

        return result;
    }

    private static void RequireArgs(string[] args, int minCount, string usage)
    {
        if (args.Length < minCount)
        {
            throw new ArgumentException($"Not enough arguments. Usage: {usage}");
        }
    }
}
