using Lab06_gamelib.Models;

namespace Lab06_gamelib.Services
{
    public class BoardService
    {
        public Board Build(int boardSize)
        {
            var board = new Board(boardSize);
            int idCounter = 1;

            for (int y = 0; y < boardSize; y++)
            {
                for (int x = 0; x < boardSize; x++)
                {
                    string fieldName = $"Sector-{idCounter}";

                    var field = new Field(
                        radius: 20, 
                        id: idCounter++,
                        name: fieldName
                    );
                    board.SetField(x, y, field);
                }
            }

            return board;
        }
    }
}
