# DVLA Golf Club Coding Exercise
This is my (Joshua Fletcher's) solution to the coding exercise, implemented in C# 10.0/.NET 10.0.


## Run it
Currently only 'total-par' and 'members-below' are implemented, but the rest of the commands are stubbed out and will print a message that they aren't implemented yet.

Commands for the application are as follows, inside the same directory that contains the solution file:
```bash
Usage: dotnet run --project .\GolfClubDVLA\GolfClubDVLA.csproj --<command> [args]
Usage: dotnet run --project .\GolfClubDVLA --<command> [args]

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
```

Tests can be run with the following command, inside the same directory that contains the solution file:
```bash
Usage: dotnet test .\GolfClubDVLA.Tests\GolfClubDVLA.Tests.csproj
Usage: dotnet test .\GolfClubDVLA.Tests
Usage: dotnet test
```