using Lab06_gamelib.Models;
using System;
using System.Collections.Generic;

namespace Lab06_gamelib.Services
{
    public static class GameWorldQueries
    {
        public static List<Planet> GetPlanets(GameState state)
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

        public static bool PlayerHasShipyard(GameState state, Player player)
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

        public static void HandleShipyardFailure(GameState state, Player player, GameSettings settings)
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

        public static PlanetarySystem? FindFirstOwnedShipyard(GameState state, Player player)
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

        public static void PrintRoundSummary(GameState state)
        {
            foreach (var player in state.Players)
            {
                int shipyards = CountShipyards(state, player);
                int asteroidLevel = SumAsteroidMineLevels(state, player);
                Console.WriteLine($"{player.Name}: credits {player.Credits}, pos ({player.X},{player.Y}), owned {player.OwnedFieldIds.Count}, shipyards {shipyards}, asteroid lvl {asteroidLevel}");
            }
        }

        public static void UpdateSystemOwnership(GameState state, int? systemId)
        {
            if (systemId == null)
            {
                return;
            }

            var system = FindSystem(state, systemId);
            if (system == null)
            {
                return;
            }

            int? ownerId = null;
            foreach (var planetId in system.PlanetFieldIds)
            {
                var planet = FindPlanetById(state, planetId);
                if (planet == null || planet.OwnerId == null)
                {
                    ownerId = null;
                    break;
                }

                if (ownerId == null)
                {
                    ownerId = planet.OwnerId;
                }
                else if (ownerId != planet.OwnerId)
                {
                    ownerId = null;
                    break;
                }
            }

            system.OwnerId = ownerId;
        }

        public static PlanetarySystem? FindSystem(GameState state, int? systemId)
        {
            if (systemId == null)
            {
                return null;
            }

            foreach (var system in state.World.Systems)
            {
                if (system.Id == systemId)
                {
                    return system;
                }
            }

            return null;
        }

        public static Planet? FindPlanetById(GameState state, int planetId)
        {
            foreach (var pos in state.World.Track)
            {
                var field = state.World.Board.GetField(pos.X, pos.Y);
                if (field is Planet planet && planet.Id == planetId)
                {
                    return planet;
                }
            }

            return null;
        }

        public static int CountShipyards(GameState state, Player player)
        {
            int count = 0;
            foreach (var system in state.World.Systems)
            {
                if (system.OwnerId == player.Id && system.HasShipyard)
                {
                    count++;
                }
            }

            return count;
        }

        public static int SumAsteroidMineLevels(GameState state, Player player)
        {
            int sum = 0;
            foreach (var system in state.World.Systems)
            {
                if (system.OwnerId == player.Id)
                {
                    sum += system.AsteroidMineLevel;
                }
            }

            return sum;
        }
    }
}
