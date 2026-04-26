using Lab06_gamelib.Models;
using Lab06_gamelib.Services;

namespace Lab06_gamelib
{
    public class GameLoop(GameLog logger, BoardService board, DiceService dice)

    {
        private Board Board { get; set; }
        
        public void Start()
        {
            logger.Log("Game loop started.");
            // initialize board 
            // initialize players
            // if (not ENDED)
                // for every player
                    // get user input
                    // do user action
                
        }

        private void Turn()
        {
            logger.Log("Game loop turn executed.");
        }

        private void Stop()
        {
            logger.Log("Game loop stopped.");
        }
    }
}
