using GolfClubDVLA.CommandRunner;
using GolfClubDVLA.Interfaces;
using GolfClubDVLA.Models;
using GolfClubDVLA.Repositories;
using GolfClubDVLA.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IGolfClubRepository, InMemoryGolfClubRepository>();
builder.Services.AddSingleton<GolfClubService>();

using IHost host = builder.Build();

// Ctrl+C cancels cleanly rather than killing the process mid-command.
using CancellationTokenSource cts = new();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();
IGolfClubRepository repository = host.Services.GetRequiredService<IGolfClubRepository>();
GolfClubService club = host.Services.GetRequiredService<GolfClubService>();

try
{
    await SeedAsync(repository, logger, cts.Token);
    await CommandRunner.RunAsync(args, club, logger, cts.Token);
}
catch (OperationCanceledException)
{
    logger.LogWarning("Cancelled.");
}
catch (ArgumentException ex)
{
    // Bad CLI input or a domain validation failure (e.g. handicap out of range) -
    // expected, not a crash-worthy bug, so log it plainly rather than a full stack trace.
    logger.LogError("{Message}", ex.Message);
}
catch (InvalidOperationException ex)
{
    // e.g. duplicate member number.
    logger.LogError("{Message}", ex.Message);
}

return;

static async Task SeedAsync(IGolfClubRepository repository, ILogger logger, CancellationToken cancellationToken)
{
    // We need to seed the in memory files, normally we'd use a database and migrations but for this demo we will just seed the data in memory.
    List<Hole> holes =
    [
        new(number: 1, par: 4),
        new(number: 2, par: 3),
        new(number: 3, par: 5),
        new(number: 4, par: 3),
    ];

    List<Member> members =
    [
        new(number: 1, name: "Jim Parr", handicap: 10),
        new(number: 2, name: "Jon Rahm", handicap: 4),
        new(number: 3, name: "Ernie Elsif", handicap: 18),
    ];

    logger.LogDebug("Seeding in-memory store...");
    await repository.SeedIfEmptyAsync(holes, members, cancellationToken);
}
