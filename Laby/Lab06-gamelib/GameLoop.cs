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

                ApplyIncomeIfDue(state, settings);

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
                ResolvePirateEncounter(player, settings);
                return;
            }

            if (field.Kind == FieldKind.RailStop)
            {
                HandleRailStop(state, player);
                return;
            }

            if (field.Kind == FieldKind.Singularity)
            {
                HandleSingularity(state, player, settings);
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

        private void HandleSingularity(GameState state, Player player, GameSettings settings)
        {
            var card = DrawSingularityCard(state, player);
            Console.WriteLine($"Singularity card: {card}");

            if (card == SingularityCardKind.PirateAttack)
            {
                ResolvePirateEncounter(player, settings);
                return;
            }

            if (card == SingularityCardKind.PirateDefense)
            {
                player.HasPirateDefenseCard = true;
                Console.WriteLine($"{player.Name} received pirate defense card.");
                return;
            }

            if (card == SingularityCardKind.GalacticTicket)
            {
                player.GalacticTickets++;
                Console.WriteLine($"{player.Name} received galactic ticket.");
                return;
            }

            if (card == SingularityCardKind.Tax)
            {
                int value = CalculatePropertyValue(state, player, settings);
                int tax = value * settings.TaxRatePercent / 100;
                int paid = Math.Min(player.Credits, tax);
                player.Credits -= paid;
                Console.WriteLine($"{player.Name} paid tax {paid}.");
                return;
            }

            if (card == SingularityCardKind.Lottery)
            {
                player.Credits += settings.LotteryReward;
                Console.WriteLine($"{player.Name} won lottery {settings.LotteryReward}.");
                return;
            }

            if (card == SingularityCardKind.EngineFailure)
            {
                player.TurnsToSkip += settings.EngineFailureTurnsLost;
                int paid = Math.Min(player.Credits, settings.EngineFailureTowCost);
                player.Credits -= paid;
                Console.WriteLine($"{player.Name} had engine failure and paid {paid}.");
                return;
            }

            if (card == SingularityCardKind.ShipyardFailure)
            {
                HandleShipyardFailure(state, player, settings);
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

            if (PlayerHasShipyard(state, player))
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
                bool useCard = player.IsHuman ? AskYesNo("Use pirate defense card?") : true;
                if (useCard)
                {
                    player.HasPirateDefenseCard = false;
                    Console.WriteLine($"{player.Name} blocked pirates with defense card.");
                    return;
                }
            }

            if (player.Credits >= settings.PirateRansomCost)
            {
                bool pay = player.IsHuman ? AskYesNo($"Pay ransom {settings.PirateRansomCost}?") : true;
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

        private void ApplyIncomeIfDue(GameState state, GameSettings settings)
        {
            if (settings.IncomeCadence <= 0)
            {
                return;
            }

            if (state.Round % settings.IncomeCadence != 0)
            {
                return;
            }

            foreach (var player in state.Players)
            {
                int income = CalculateIncome(state, player, settings);
                player.Credits += income;
                Console.WriteLine($"{player.Name} received income {income}.");
            }
        }

        private int CalculateIncome(GameState state, Player player, GameSettings settings)
        {
            int income = 0;

            foreach (var planet in GetPlanets(state))
            {
                if (planet.OwnerId != player.Id)
                {
                    continue;
                }

                if (planet.HasPort)
                {
                    income += settings.IncomePerPort;
                }

                income += planet.SettlementLevel * settings.IncomePerSettlementLevel;
                income += planet.MineLevel * settings.IncomePerMineLevel;
                income += planet.FarmLevel * settings.IncomePerFarmLevel;
            }

            foreach (var system in state.World.Systems)
            {
                if (system.OwnerId != player.Id)
                {
                    continue;
                }

                if (system.HasShipyard)
                {
                    income += settings.IncomePerShipyard;
                }

                income += system.AsteroidMineLevel * settings.IncomePerAsteroidMineLevel;
            }

            return income;
        }

        private int CalculatePropertyValue(GameState state, Player player, GameSettings settings)
        {
            int value = 0;

            foreach (var planet in GetPlanets(state))
            {
                if (planet.OwnerId != player.Id)
                {
                    continue;
                }

                if (planet.HasPort)
                {
                    value += settings.PortCost;
                }

                value += SumUpgradeCosts(settings.SettlementUpgradeCosts, planet.SettlementLevel);
                value += SumUpgradeCosts(settings.MineUpgradeCosts, planet.MineLevel);
                value += SumUpgradeCosts(settings.FarmUpgradeCosts, planet.FarmLevel);
            }

            foreach (var system in state.World.Systems)
            {
                if (system.OwnerId != player.Id)
                {
                    continue;
                }

                if (system.HasShipyard)
                {
                    value += settings.ShipyardCost;
                }

                value += SumUpgradeCosts(settings.AsteroidMineUpgradeCosts, system.AsteroidMineLevel);
            }

            return value;
        }

        private int SumUpgradeCosts(List<int> costs, int level)
        {
            int sum = 0;
            int cappedLevel = Math.Min(level, costs.Count);
            for (int i = 0; i < cappedLevel; i++)
            {
                sum += costs[i];
            }

            return sum;
        }

        private List<Planet> GetPlanets(GameState state)
        {
            var planets = new List<Planet>();
            foreach (var pos in state.World.Track)
            {
                var field = state.World.Board.GetField(pos.X, pos.Y);
                if (field is Planet planet)
                {
                    planets.Add(planet);
                }
            }

            return planets;
        }

        private bool PlayerHasShipyard(GameState state, Player player)
        {
            foreach (var system in state.World.Systems)
            {
                if (system.OwnerId == player.Id && system.HasShipyard)
                {
                    return true;
                }
            }

            return false;
        }

        private void HandleShipyardFailure(GameState state, Player player, GameSettings settings)
        {
            var system = FindFirstOwnedShipyard(state, player);
            if (system == null)
            {
                return;
            }

            if (player.Credits >= settings.ShipyardFailureCost)
            {
                player.Credits -= settings.ShipyardFailureCost;
                Console.WriteLine($"{player.Name} paid shipyard repair {settings.ShipyardFailureCost}.");
                return;
            }

            system.HasShipyard = false;
            Console.WriteLine($"{player.Name} lost a shipyard in {system.Name}.");
        }

        private PlanetarySystem? FindFirstOwnedShipyard(GameState state, Player player)
        {
            foreach (var system in state.World.Systems)
            {
                if (system.OwnerId == player.Id && system.HasShipyard)
                {
                    return system;
                }
            }

            return null;
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
