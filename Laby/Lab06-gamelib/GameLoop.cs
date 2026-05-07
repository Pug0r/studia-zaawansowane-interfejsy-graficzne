using Lab06_gamelib.Models;
using Lab06_gamelib.Services;
using System;
using System.Collections.Generic;

namespace Lab06_gamelib
{
    public class GameLoop(GameLog logger, BoardService boardService, DiceService dice, SettingsProvider settingsProvider)

    {
        public GameState Start()
        {
            var settings = settingsProvider.Settings;
            var world = boardService.BuildWorld(settings);
            var players = CreatePlayers(settings.StartingCredits);
            var state = new GameState(players, world);

            for (int round = 1; round <= settings.RoundLimit; round++)
            {
                state.Round = round;
                Console.WriteLine($"Round {round}");

                foreach (var player in players)
                {
                    ExecuteTurn(state, player, settings);
                }

                PrintRoundSummary(state);
            }

            return state;
        }

        private List<Player> CreatePlayers(int startingCredits)
        {
            var players = new List<Player>
            {
                new Player(1, "Player", true),
                new Player(2, "Bot A", false),
                new Player(3, "Bot B", false),
                new Player(4, "Bot C", false)
            };

            foreach (var player in players)
            {
                player.Credits = startingCredits;
            }

            return players;
        }

        private void ExecuteTurn(GameState state, Player player, GameSettings settings)
        {
            if (player.TurnsToSkip > 0)
            {
                player.TurnsToSkip--;
                Console.WriteLine($"{player.Name} skips a turn.");
                return;
            }

            if (player.IsHuman && player.ConsecutiveSkips < 2)
            {
                Console.WriteLine($"{player.Name} credits: {player.Credits}");
                Console.Write("Press Enter to roll or type skip: ");
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && input.Trim().Equals("skip", StringComparison.OrdinalIgnoreCase))
                {
                    player.ConsecutiveSkips++;
                    Console.WriteLine($"{player.Name} skips by choice.");
                    return;
                }
            }

            player.ConsecutiveSkips = 0;
            int roll = dice.Roll(1, 6);
            MovePlayer(state.World, player, roll);

            var field = state.World.Board.GetField(player.X, player.Y);
            Console.WriteLine($"{player.Name} rolled {roll} and landed on {field.Name} ({field.Kind}).");

            ResolveField(state, player, field, settings);
        }

        private void MovePlayer(GameWorld world, Player player, int roll)
        {
            if (world.Track.Count == 0)
            {
                return;
            }

            int nextIndex = (player.TrackIndex + roll) % world.Track.Count;
            player.TrackIndex = nextIndex;
            var pos = world.Track[nextIndex];
            player.X = pos.X;
            player.Y = pos.Y;
        }

        private void ResolveField(GameState state, Player player, Field field, GameSettings settings)
        {
            if (field.Kind == FieldKind.PirateAttack)
            {
                player.TurnsToSkip += settings.PirateTurnsLost;
                Console.WriteLine($"{player.Name} hit pirate attack and loses {settings.PirateTurnsLost} turns.");
                return;
            }

            if (field.Kind == FieldKind.RailStop)
            {
                HandleRailStop(state, player);
                return;
            }

            if (field.Kind == FieldKind.Singularity)
            {
                Console.WriteLine("Singularity encountered.");
                return;
            }

            if (field is Planet planet)
            {
                HandlePlanet(state, player, planet, settings);
            }
        }

        private void HandleRailStop(GameState state, Player player)
        {
            if (player.GalacticTickets <= 0)
            {
                return;
            }

            if (player.IsHuman)
            {
                Console.WriteLine($"{player.Name} has a ticket. Choose rail stop index or press Enter to skip.");
                for (int i = 0; i < state.World.RailStops.Count; i++)
                {
                    var pos = state.World.RailStops[i];
                    Console.WriteLine($"{i}: ({pos.X},{pos.Y})");
                }
                Console.Write("Choice: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int index) && index >= 0 && index < state.World.RailStops.Count)
                {
                    var pos = state.World.RailStops[index];
                    player.X = pos.X;
                    player.Y = pos.Y;
                    player.TrackIndex = state.World.Track.FindIndex(p => p.X == pos.X && p.Y == pos.Y);
                    player.GalacticTickets--;
                    Console.WriteLine($"{player.Name} moved by rail.");
                }
            }
            else
            {
                var pos = state.World.RailStops[0];
                player.X = pos.X;
                player.Y = pos.Y;
                player.TrackIndex = state.World.Track.FindIndex(p => p.X == pos.X && p.Y == pos.Y);
                player.GalacticTickets--;
            }
        }

        private void HandlePlanet(GameState state, Player player, Planet planet, GameSettings settings)
        {
            bool actionUsed = false;

            if (!planet.HasPort)
            {
                if (player.Credits >= settings.PortCost)
                {
                    bool buy = player.IsHuman ? AskYesNo("Build port?") : true;
                    if (buy)
                    {
                        player.Credits -= settings.PortCost;
                        planet.HasPort = true;
                        planet.OwnerId = player.Id;
                        player.OwnedFieldIds.Add(planet.Id);
                        actionUsed = true;
                        Console.WriteLine($"{player.Name} built a port on {planet.Name}.");
                    }
                }
                return;
            }

            if (planet.OwnerId != player.Id)
            {
                return;
            }

            if (actionUsed)
            {
                return;
            }

            if (player.IsHuman)
            {
                Console.WriteLine("Choose upgrade: 1-settlement, 2-mine, 3-farm, Enter to skip");
                var input = Console.ReadLine();
                if (input == "1")
                {
                    TryUpgradeSettlement(player, planet, settings);
                }
                else if (input == "2")
                {
                    TryUpgradeMine(player, planet, settings);
                }
                else if (input == "3")
                {
                    TryUpgradeFarm(player, planet, settings);
                }
            }
            else
            {
                if (!TryUpgradeSettlement(player, planet, settings))
                {
                    if (!TryUpgradeMine(player, planet, settings))
                    {
                        TryUpgradeFarm(player, planet, settings);
                    }
                }
            }
        }

        private bool TryUpgradeSettlement(Player player, Planet planet, GameSettings settings)
        {
            if (planet.SettlementLevel >= settings.SettlementUpgradeCosts.Count)
            {
                return false;
            }

            int cost = settings.SettlementUpgradeCosts[planet.SettlementLevel];
            if (player.Credits < cost)
            {
                return false;
            }

            player.Credits -= cost;
            planet.SettlementLevel++;
            Console.WriteLine($"{player.Name} upgraded settlement to {planet.SettlementLevel}.");
            return true;
        }

        private bool TryUpgradeMine(Player player, Planet planet, GameSettings settings)
        {
            if (planet.MineLevel >= settings.MineUpgradeCosts.Count)
            {
                return false;
            }

            int cost = settings.MineUpgradeCosts[planet.MineLevel];
            if (player.Credits < cost)
            {
                return false;
            }

            player.Credits -= cost;
            planet.MineLevel++;
            Console.WriteLine($"{player.Name} upgraded mine to {planet.MineLevel}.");
            return true;
        }

        private bool TryUpgradeFarm(Player player, Planet planet, GameSettings settings)
        {
            if (planet.FarmLevel >= settings.FarmUpgradeCosts.Count)
            {
                return false;
            }

            int cost = settings.FarmUpgradeCosts[planet.FarmLevel];
            if (player.Credits < cost)
            {
                return false;
            }

            player.Credits -= cost;
            planet.FarmLevel++;
            Console.WriteLine($"{player.Name} upgraded farm to {planet.FarmLevel}.");
            return true;
        }

        private bool AskYesNo(string prompt)
        {
            Console.Write($"{prompt} (y/n): ");
            var input = Console.ReadLine();
            return input != null && input.Trim().Equals("y", StringComparison.OrdinalIgnoreCase);
        }

        private void PrintRoundSummary(GameState state)
        {
            foreach (var player in state.Players)
            {
                Console.WriteLine($"{player.Name}: credits {player.Credits}, pos ({player.X},{player.Y})");
            }
        }
    }
}
