using System.Collections.Generic;

namespace Lab06_gamelib.Models
{
    public class GameWorld
    {
        public Board Board { get; private set; }
        public List<PlanetarySystem> Systems { get; private set; }
        public List<BoardPosition> Track { get; private set; }
        public List<BoardPosition> RailStops { get; private set; }

        public GameWorld(Board board, List<PlanetarySystem> systems, List<BoardPosition> track, List<BoardPosition> railStops)
        {
            Board = board;
            Systems = systems;
            Track = track;
            RailStops = railStops;
        }
    }
}
