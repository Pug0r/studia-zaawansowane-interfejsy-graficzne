using Lab06_gamelib;
using Lab06_gamelib.Services;
using Lab06_gamelib.Models;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddSingleton<GameLog>()
    .AddSingleton<GameLoop>()
    .AddSingleton<BoardService>()
    .AddSingleton<SettingsProvider>()
    .AddSingleton<DiceService>()
    .BuildServiceProvider();

var engine = serviceProvider.GetRequiredService<GameLoop>();

Console.WriteLine("Game started. Press Ctrl+C to stop.");
var state = engine.Start();
Console.WriteLine("Game finished.");
foreach (var player in state.Players)
{
    Console.WriteLine($"{player.Name}: credits {player.Credits}, owned {player.OwnedFieldIds.Count}");
}
       