using Lab06_gamelib.Models;
using System;
using System.Collections.Generic;

namespace Lab06_gamelib.Services
{
    public static class GameEconomy
    {
        public static void ApplyIncomeIfDue(GameState state, GameSettings settings)
        {
            if (settings.IncomeCadence <= 0 || state.Round % settings.IncomeCadence != 0)
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

        public static int CalculateIncome(GameState state, Player player, GameSettings settings)
        {
            int income = 0;

            foreach (var planet in GameWorldQueries.GetPlanets(state))
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

        public static int CalculatePropertyValue(GameState state, Player player, GameSettings settings)
        {
            int value = 0;

            foreach (var planet in GameWorldQueries.GetPlanets(state))
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

        public static int SumUpgradeCosts(List<int> costs, int level)
        {
            int sum = 0;
            int cappedLevel = Math.Min(level, costs.Count);
            for (int i = 0; i < cappedLevel; i++)
            {
                sum += costs[i];
            }

            return sum;
        }
    }
}
