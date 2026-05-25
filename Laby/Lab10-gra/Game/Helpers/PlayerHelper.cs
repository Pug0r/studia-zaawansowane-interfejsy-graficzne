using Lab06_gamelib.Models;
using Lab06_gamelib.Services;
using Lab10_gra.Game;
using System;
using System.Collections.Generic;

namespace Lab10_gra.Game.Helpers
{
    public class PlayerHelper(DiceService dice)
    {
        public List<Player> CreatePlayers(int startingCredits)
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

        public bool CanHandlePlanet(Player player, Planet planet, Lab06_gamelib.GameSettings settings)
        {
            if (!planet.HasPort && player.Credits >= settings.PortCost)
            {
                return true;
            }

            if (!planet.HasPort || planet.OwnerId != player.Id)
            {
                return false;
            }

            return true;
        }

        public bool TryBuildPort(Player player, Planet planet, Lab06_gamelib.GameSettings settings)
        {
            if (player.Credits < settings.PortCost)
            {
                return false;
            }

            player.Credits -= settings.PortCost;
            planet.HasPort = true;
            planet.OwnerId = player.Id;
            player.OwnedFieldIds.Add(planet.Id);
            return true;
        }

        public void ApplyUpgradeDecision(Player player, Planet planet, PlanetarySystem? system, Lab06_gamelib.GameSettings settings, DecisionType decision, Action<string> log)
        {
            switch (decision)
            {
                case DecisionType.UpgradeSettlement:
                    TryUpgradePlanetLevel(player, planet, settings.SettlementUpgradeCosts, p => p.SettlementLevel, (p, level) => p.SettlementLevel = level, "settlement", log);
                    break;
                case DecisionType.UpgradeMine:
                    TryUpgradePlanetLevel(player, planet, settings.MineUpgradeCosts, p => p.MineLevel, (p, level) => p.MineLevel = level, "mine", log);
                    break;
                case DecisionType.UpgradeFarm:
                    TryUpgradePlanetLevel(player, planet, settings.FarmUpgradeCosts, p => p.FarmLevel, (p, level) => p.FarmLevel = level, "farm", log);
                    break;
                case DecisionType.BuildShipyard when system != null:
                    TryBuildShipyard(player, system, settings, log);
                    break;
                case DecisionType.UpgradeAsteroidMine when system != null:
                    TryUpgradeAsteroidMine(player, system, settings, log);
                    break;
            }
        }

        public void ApplyBotSystemUpgrade(Player player, Planet planet, PlanetarySystem system, Lab06_gamelib.GameSettings settings, Action<string> log)
        {
            var options = new List<Func<bool>>
            {
                () => TryBuildShipyard(player, system, settings, log),
                () => TryUpgradeAsteroidMine(player, system, settings, log),
                () => TryUpgradePlanetLevel(player, planet, settings.SettlementUpgradeCosts, p => p.SettlementLevel, (p, level) => p.SettlementLevel = level, "settlement", log),
                () => TryUpgradePlanetLevel(player, planet, settings.MineUpgradeCosts, p => p.MineLevel, (p, level) => p.MineLevel = level, "mine", log),
                () => TryUpgradePlanetLevel(player, planet, settings.FarmUpgradeCosts, p => p.FarmLevel, (p, level) => p.FarmLevel = level, "farm", log)
            };
            int index = dice.Roll(0, options.Count - 1);
            options[index]();
        }

        public void ApplyBotBasicUpgrade(Player player, Planet planet, Lab06_gamelib.GameSettings settings, Action<string> log)
        {
            if (!TryUpgradePlanetLevel(player, planet, settings.SettlementUpgradeCosts, p => p.SettlementLevel, (p, level) => p.SettlementLevel = level, "settlement", log)
                && !TryUpgradePlanetLevel(player, planet, settings.MineUpgradeCosts, p => p.MineLevel, (p, level) => p.MineLevel = level, "mine", log))
            {
                TryUpgradePlanetLevel(player, planet, settings.FarmUpgradeCosts, p => p.FarmLevel, (p, level) => p.FarmLevel = level, "farm", log);
            }
        }

        private bool TryUpgradePlanetLevel(Player player, Planet planet, List<int> costs, Func<Planet, int> getLevel, Action<Planet, int> setLevel, string label, Action<string> log)
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
            log($"{player.Name} upgraded {label} to {newLevel}.");
            return true;
        }

        private bool TryBuildShipyard(Player player, PlanetarySystem system, Lab06_gamelib.GameSettings settings, Action<string> log)
        {
            if (system.HasShipyard || player.Credits < settings.ShipyardCost)
            {
                return false;
            }

            player.Credits -= settings.ShipyardCost;
            system.HasShipyard = true;
            log($"{player.Name} built a shipyard in {system.Name}.");
            return true;
        }

        private bool TryUpgradeAsteroidMine(Player player, PlanetarySystem system, Lab06_gamelib.GameSettings settings, Action<string> log)
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
            log($"{player.Name} upgraded asteroid mine to {system.AsteroidMineLevel} in {system.Name}.");
            return true;
        }
    }
}
