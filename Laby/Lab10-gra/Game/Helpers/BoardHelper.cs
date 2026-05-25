using Lab06_gamelib.Models;
using Lab06_gamelib.Services;

namespace Lab10_gra.Game.Helpers
{
    public class BoardHelper()
    {
        public void MovePlayer(GameWorld world, Player player, int roll)
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

        public bool TryMoveByRail(GameWorld world, Player player)
        {
            if (world.RailStops.Count == 0)
            {
                return false;
            }

            var pos = world.RailStops[0];
            player.X = pos.X;
            player.Y = pos.Y;
            player.TrackIndex = world.Track.FindIndex(p => p.X == pos.X && p.Y == pos.Y);
            player.GalacticTickets--;
            return true;
        }
    }
}
