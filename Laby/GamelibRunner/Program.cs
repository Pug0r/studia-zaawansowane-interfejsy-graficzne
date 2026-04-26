using Lab06_gamelib;
using Lab06_gamelib.Services;
using Lab06_gamelib.Models;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");


var serviceProvider = new ServiceCollection()
    .AddSingleton<GameLog>()
    .AddSingleton<GameLoop>()
    .AddSingleton<BoardService>()
    .AddSingleton<SettingsProvider>()
    .AddSingleton<DiceService>()
    .BuildServiceProvider();

var gameSettings = serviceProvider.GetService<SettingsProvider>()!.Settings;
var board = serviceProvider.GetRequiredService<BoardService>().Build(gameSettings.BoardSize);
var engine = serviceProvider.GetRequiredService<GameLoop>();

Console.WriteLine("Game started. Press Ctrl+C to stop.");
engine.Start();
       