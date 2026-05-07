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
            var systems = new List<PlanetarySystem>
            {
                new PlanetarySystem(1, "System-1"),
                new PlanetarySystem(2, "System-2"),
                new PlanetarySystem(3, "System-3"),
                new PlanetarySystem(4, "System-4"),
                new PlanetarySystem(5, "System-5")
            };
            var track = BuildTrack(BoardSize);
            var railStops = new List<BoardPosition>();
            var layout = new (FieldKind kind, int? systemId)[]
            {
                (FieldKind.RailStop, null), (FieldKind.Planet, 1), (FieldKind.Planet, 1), (FieldKind.Planet, 1), (FieldKind.RailStop, null),
                (FieldKind.Planet, 2), (FieldKind.Planet, 2), (FieldKind.PirateAttack, null), (FieldKind.Planet, 2), (FieldKind.Planet, 2),
                (FieldKind.Planet, 3), (FieldKind.Planet, 3), (FieldKind.Singularity, null), (FieldKind.Planet, 3), (FieldKind.Planet, 3),
                (FieldKind.Planet, 4), (FieldKind.PirateAttack, null), (FieldKind.Planet, 4), (FieldKind.Planet, 4), (FieldKind.Planet, 4),
                (FieldKind.RailStop, null), (FieldKind.Planet, 5), (FieldKind.Planet, 5), (FieldKind.Planet, 5), (FieldKind.RailStop, null)
            };
            int idCounter = 1;

            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    int index = y * BoardSize + x;
                    string fieldName = $"Sector-{idCounter}";
                    var (kind, systemId) = layout[index];
                    Field field;

                    if (kind == FieldKind.Planet)
                    {
                        int resolvedSystemId = systemId ?? 1;
                        var planet = new Planet(idCounter++, fieldName, resolvedSystemId);
                        systems[resolvedSystemId - 1].PlanetFieldIds.Add(planet.Id);
                        field = planet;
                    }
                    else
                    {
                        field = new Field(idCounter++, fieldName, kind);
                        if (kind == FieldKind.RailStop)
                        {
                            railStops.Add(new BoardPosition(x, y));
                        }
                    }

                    board.SetField(x, y, field);
                }
            }

            return new GameWorld(board, systems, track, railStops);
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

    }
}
