using Lab06_gamelib.Models;
using System.Collections.Generic;

namespace Lab06_gamelib.Services
{
    public class BoardService
    {
        private const int BoardSize = 5;

        public GameWorld BuildWorld(GameSettings settings)
        {
            var board = new Board(BoardSize);
            var systems = new List<PlanetarySystem>();
            var track = BuildTrack(BoardSize);
            var railStops = new List<BoardPosition>();
            int idCounter = 1;

            for (int y = 0; y < BoardSize; y++)
            {
                var system = new PlanetarySystem(y + 1, $"System-{y + 1}");
                systems.Add(system);

                for (int x = 0; x < BoardSize; x++)
                {
                    string fieldName = $"Sector-{idCounter}";
                    Field field;

                    if (IsSingularity(x, y))
                    {
                        field = new Field(20, idCounter++, fieldName, FieldKind.Singularity);
                    }
                    else if (IsPirateZone(x, y))
                    {
                        field = new Field(20, idCounter++, fieldName, FieldKind.PirateAttack);
                    }
                    else if (IsRailStop(x, y))
                    {
                        field = new Field(20, idCounter++, fieldName, FieldKind.RailStop);
                        railStops.Add(new BoardPosition(x, y));
                    }
                    else
                    {
                        int richness = 1 + (idCounter % 3);
                        var planet = new Planet(20, idCounter++, fieldName, richness, system.Id);
                        system.PlanetFieldIds.Add(planet.Id);
                        field = planet;
                    }

                    board.SetField(x, y, field);
                }
            }

            return new GameWorld(board, systems, track, railStops);
        }

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
                        name: fieldName,
                        kind: FieldKind.Empty
                    );
                    board.SetField(x, y, field);
                }
            }

            return board;
        }

        private static List<BoardPosition> BuildTrack(int size)
        {
            var track = new List<BoardPosition>();

            for (int y = 0; y < size; y++)
            {
                if (y % 2 == 0)
                {
                    for (int x = 0; x < size; x++)
                    {
                        track.Add(new BoardPosition(x, y));
                    }
                }
                else
                {
                    for (int x = size - 1; x >= 0; x--)
                    {
                        track.Add(new BoardPosition(x, y));
                    }
                }
            }

            return track;
        }

        private static bool IsSingularity(int x, int y)
        {
            return x == BoardSize / 2 && y == BoardSize / 2;
        }

        private static bool IsPirateZone(int x, int y)
        {
            return (x == 1 && y == 3) || (x == 3 && y == 1);
        }

        private static bool IsRailStop(int x, int y)
        {
            return (x == 0 && y == 0) || (x == BoardSize - 1 && y == 0) || (x == 0 && y == BoardSize - 1) || (x == BoardSize - 1 && y == BoardSize - 1);
        }
    }
}
