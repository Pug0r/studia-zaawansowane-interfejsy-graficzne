using System.Collections.Generic;

namespace Lab10_gra.EndGame
{
    public class EndGameViewModel
    {
        public IReadOnlyList<PlayerResultViewModel> Results { get; }

        public EndGameViewModel(IReadOnlyList<PlayerResultViewModel> results)
        {
            Results = results;
        }
    }

    public class PlayerResultViewModel
    {
        public int Rank { get; }
        public string Name { get; }
        public int Credits { get; }
        public int OwnedFields { get; }
        public int Shipyards { get; }
        public int AsteroidMineLevel { get; }

        public PlayerResultViewModel(int rank, string name, int credits, int ownedFields, int shipyards, int asteroidMineLevel)
        {
            Rank = rank;
            Name = name;
            Credits = credits;
            OwnedFields = ownedFields;
            Shipyards = shipyards;
            AsteroidMineLevel = asteroidMineLevel;
        }
    }
}
