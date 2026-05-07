using Lab06_gamelib.Models;
using Lab06_gamelib.Services;
using System;
using System.Collections.Generic;

namespace Lab06_gamelib
{
    public class GameLoop(BoardService boardService, DiceService dice, SettingsProvider settingsProvider)

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

                GameEconomy.ApplyIncomeIfDue(state, settings);

                GameWorldQueries.PrintRoundSummary(state);
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
                if (GameWorldQueries.AskChoice("Choose action: 1-roll, 2-skip", 1, 2, true) == 2)
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
            switch (field)
            {
                case { Kind: FieldKind.PirateAttack }:
                    ResolvePirateEncounter(player, settings);
                    return;
                case { Kind: FieldKind.RailStop }:
                    HandleRailStop(state, player);
                    return;
                case { Kind: FieldKind.Singularity }:
                    HandleSingularity(state, player, settings);
                    return;
                case Planet planet:
                    HandlePlanet(state, player, planet, settings);
                    return;
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
            }
            int? index = player.IsHuman ? GameWorldQueries.AskChoice("Rail stop index", 0, state.World.RailStops.Count - 1, true) : 0;
            if (index != null)
            {
                var pos = state.World.RailStops[index.Value];
                player.X = pos.X;
                player.Y = pos.Y;
                player.TrackIndex = state.World.Track.FindIndex(p => p.X == pos.X && p.Y == pos.Y);
                player.GalacticTickets--;
                Console.WriteLine($"{player.Name} moved by rail.");
            }
        }

        private void HandleSingularity(GameState state, Player player, GameSettings settings)
        {
            var card = DrawSingularityCard(state, player);
            Console.WriteLine($"Singularity card: {card}");
            switch (card)
            {
                case SingularityCardKind.PirateAttack:
                    ResolvePirateEncounter(player, settings);
                    break;
                case SingularityCardKind.PirateDefense:
                    player.HasPirateDefenseCard = true;
                    Console.WriteLine($"{player.Name} received pirate defense card.");
                    break;
                case SingularityCardKind.GalacticTicket:
                    player.GalacticTickets++;
                    Console.WriteLine($"{player.Name} received galactic ticket.");
                    break;
                case SingularityCardKind.Tax:
                    int value = GameEconomy.CalculatePropertyValue(state, player, settings);
                    int tax = value * settings.TaxRatePercent / 100;
                    int paid = Math.Min(player.Credits, tax);
                    player.Credits -= paid;
                    Console.WriteLine($"{player.Name} paid tax {paid}.");
                    break;
                case SingularityCardKind.Lottery:
                    player.Credits += settings.LotteryReward;
                    Console.WriteLine($"{player.Name} won lottery {settings.LotteryReward}.");
                    break;
                case SingularityCardKind.EngineFailure:
                    player.TurnsToSkip += settings.EngineFailureTurnsLost;
                    int failurePaid = Math.Min(player.Credits, settings.EngineFailureTowCost);
                    player.Credits -= failurePaid;
                    Console.WriteLine($"{player.Name} had engine failure and paid {failurePaid}.");
                    break;
                case SingularityCardKind.ShipyardFailure:
                    GameWorldQueries.HandleShipyardFailure(state, player, settings);
                    break;
            }
        }

        private SingularityCardKind DrawSingularityCard(GameState state, Player player)
        {
            var cards = new List<SingularityCardKind>
            {
                SingularityCardKind.PirateAttack,
                SingularityCardKind.PirateDefense,
                SingularityCardKind.GalacticTicket,
                SingularityCardKind.Tax,
                SingularityCardKind.Lottery,
                SingularityCardKind.EngineFailure
            };

            if (GameWorldQueries.PlayerHasShipyard(state, player))
            {
                cards.Add(SingularityCardKind.ShipyardFailure);
            }

            int index = dice.Roll(0, cards.Count - 1);
            return cards[index];
        }

        private void ResolvePirateEncounter(Player player, GameSettings settings)
        {
            if (player.HasPirateDefenseCard)
            {
                bool useCard = player.IsHuman ? GameWorldQueries.AskChoice("Use pirate defense card? 1-yes 2-no", 1, 2, false) == 1 : true;
                if (useCard)
                {
                    player.HasPirateDefenseCard = false;
                    Console.WriteLine($"{player.Name} blocked pirates with defense card.");
                    return;
                }
            }

            if (player.Credits >= settings.PirateRansomCost)
            {
                bool pay = player.IsHuman ? GameWorldQueries.AskChoice($"Pay ransom {settings.PirateRansomCost}? 1-yes 2-no", 1, 2, false) == 1 : true;
                if (pay)
                {
                    player.Credits -= settings.PirateRansomCost;
                    Console.WriteLine($"{player.Name} paid ransom.");
                    return;
                }
            }

            player.TurnsToSkip += settings.PirateTurnsLost;
            Console.WriteLine($"{player.Name} loses {settings.PirateTurnsLost} turns.");
        }

        private void HandlePlanet(GameState state, Player player, Planet planet, GameSettings settings)
        {
            if (!planet.HasPort)
            {
                if (player.Credits >= settings.PortCost)
                {
                    bool buy = player.IsHuman ? GameWorldQueries.AskChoice("Build port? 1-yes 2-no", 1, 2, false) == 1 : true;
                    if (buy)
                    {
                        player.Credits -= settings.PortCost;
                        planet.HasPort = true;
                        planet.OwnerId = player.Id;
                        player.OwnedFieldIds.Add(planet.Id);
                        Console.WriteLine($"{player.Name} built a port on {planet.Name}.");
                        GameWorldQueries.UpdateSystemOwnership(state, planet.SystemId);
                    }
                }
                return;
            }

            if (planet.OwnerId != player.Id)
            {
                return;
            }

            var system = GameWorldQueries.FindSystem(state, planet.SystemId);
            if (system != null && system.OwnerId == player.Id)
            {
                if (player.IsHuman)
                {
                    int? input = GameWorldQueries.AskChoice("Choose upgrade: 1-settlement, 2-mine, 3-farm, 4-shipyard, 5-asteroid mine", 1, 5, true);
                    switch (input)
                    {
                        case 1:
                            TryUpgradePlanetLevel(player, planet, settings.SettlementUpgradeCosts, p => p.SettlementLevel, (p, level) => p.SettlementLevel = level, "settlement");
                            break;
                        case 2:
                            TryUpgradePlanetLevel(player, planet, settings.MineUpgradeCosts, p => p.MineLevel, (p, level) => p.MineLevel = level, "mine");
                            break;
                        case 3:
                            TryUpgradePlanetLevel(player, planet, settings.FarmUpgradeCosts, p => p.FarmLevel, (p, level) => p.FarmLevel = level, "farm");
                            break;
                        case 4:
                            TryBuildShipyard(player, system, settings);
                            break;
                        case 5:
                            TryUpgradeAsteroidMine(player, system, settings);
                            break;
                    }
                }
                else
                {
                    var options = new List<Func<bool>>
                    {
                        () => TryBuildShipyard(player, system, settings),
                        () => TryUpgradeAsteroidMine(player, system, settings),
                        () => TryUpgradePlanetLevel(player, planet, settings.SettlementUpgradeCosts, p => p.SettlementLevel, (p, level) => p.SettlementLevel = level, "settlement"),
                        () => TryUpgradePlanetLevel(player, planet, settings.MineUpgradeCosts, p => p.MineLevel, (p, level) => p.MineLevel = level, "mine"),
                        () => TryUpgradePlanetLevel(player, planet, settings.FarmUpgradeCosts, p => p.FarmLevel, (p, level) => p.FarmLevel = level, "farm")
                    };
                    int index = dice.Roll(0, options.Count - 1);
                    options[index]();
                }

                return;
            }

            if (player.IsHuman)
            {
                int? input = GameWorldQueries.AskChoice("Choose upgrade: 1-settlement, 2-mine, 3-farm", 1, 3, true);
                switch (input)
                {
                    case 1:
                        TryUpgradePlanetLevel(player, planet, settings.SettlementUpgradeCosts, p => p.SettlementLevel, (p, level) => p.SettlementLevel = level, "settlement");
                        break;
                    case 2:
                        TryUpgradePlanetLevel(player, planet, settings.MineUpgradeCosts, p => p.MineLevel, (p, level) => p.MineLevel = level, "mine");
                        break;
                    case 3:
                        TryUpgradePlanetLevel(player, planet, settings.FarmUpgradeCosts, p => p.FarmLevel, (p, level) => p.FarmLevel = level, "farm");
                        break;
                }
            }
            else
            {
                if (!TryUpgradePlanetLevel(player, planet, settings.SettlementUpgradeCosts, p => p.SettlementLevel, (p, level) => p.SettlementLevel = level, "settlement")
                    && !TryUpgradePlanetLevel(player, planet, settings.MineUpgradeCosts, p => p.MineLevel, (p, level) => p.MineLevel = level, "mine"))
                {
                    TryUpgradePlanetLevel(player, planet, settings.FarmUpgradeCosts, p => p.FarmLevel, (p, level) => p.FarmLevel = level, "farm");
                }
            }
        }

        private bool TryUpgradePlanetLevel(Player player, Planet planet, List<int> costs, Func<Planet, int> getLevel, Action<Planet, int> setLevel, string label)
        {
            int level = getLevel(planet);
            if (level >= costs.Count)
            {
                return false;
            }

            int cost = costs[level];
            if (player.Credits < cost)
            {
                return false;
            }

            player.Credits -= cost;
            int newLevel = level + 1;
            setLevel(planet, newLevel);
            Console.WriteLine($"{player.Name} upgraded {label} to {newLevel}.");
            return true;
        }

        private bool TryBuildShipyard(Player player, PlanetarySystem system, GameSettings settings)
        {
            if (system.HasShipyard)
            {
                return false;
            }

            if (player.Credits < settings.ShipyardCost)
            {
                return false;
            }

            player.Credits -= settings.ShipyardCost;
            system.HasShipyard = true;
            Console.WriteLine($"{player.Name} built a shipyard in {system.Name}.");
            return true;
        }

        private bool TryUpgradeAsteroidMine(Player player, PlanetarySystem system, GameSettings settings)
        {
            if (system.AsteroidMineLevel >= settings.AsteroidMineUpgradeCosts.Count)
            {
                return false;
            }

            int cost = settings.AsteroidMineUpgradeCosts[system.AsteroidMineLevel];
            if (player.Credits < cost)
            {
                return false;
            }

            player.Credits -= cost;
            system.AsteroidMineLevel++;
            Console.WriteLine($"{player.Name} upgraded asteroid mine to {system.AsteroidMineLevel} in {system.Name}.");
            return true;
        }
    }
}
