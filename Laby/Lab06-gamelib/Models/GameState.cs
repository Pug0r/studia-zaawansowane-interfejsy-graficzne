using System.Collections.Generic;

namespace Lab06_gamelib.Models
{
    public class GameState
    {
        public int Round { get; set; }
        public List<Player> Players { get; private set; }
        public GameWorld World { get; private set; }

        public GameState(List<Player> players, GameWorld world)
        {
            Players = players;
            World = world;
        }
    }
}
