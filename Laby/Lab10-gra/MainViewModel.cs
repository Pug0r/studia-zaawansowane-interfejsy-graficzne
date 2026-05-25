using Lab06_gamelib;
using Lab06_gamelib.Models;
using Lab06_gamelib.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace Lab10_gra
{
    public class MainViewModel
    {
        public ObservableCollection<BoardCellViewModel> BoardCells { get; } = new();
        public string RoundLabel { get; }
        public string PlayerLabel { get; }
        public string CreditsLabel { get; }
        public string GalacticTicketsLabel { get; }
        public string PirateDefenseCardLabel { get; }
        public string LogLabel { get; }
        public Visibility TakeTurnVisibility { get; } = Visibility.Visible;
        public Visibility SkipTurnVisibility { get; } = Visibility.Visible;
        public Visibility PayRansomVisibility { get; } = Visibility.Collapsed;
        public Visibility UsePirateDefenseVisibility { get; } = Visibility.Collapsed;
        public Visibility DeclinePirateDefenseVisibility { get; } = Visibility.Collapsed;
        public Visibility RailStopVisibility { get; } = Visibility.Collapsed;
        public Visibility BuildPortVisibility { get; } = Visibility.Collapsed;
        public Visibility UpgradeSettlementVisibility { get; } = Visibility.Collapsed;
        public Visibility UpgradeMineVisibility { get; } = Visibility.Collapsed;
        public Visibility UpgradeFarmVisibility { get; } = Visibility.Collapsed;
        public Visibility BuildShipyardVisibility { get; } = Visibility.Collapsed;
        public Visibility UpgradeAsteroidMineVisibility { get; } = Visibility.Collapsed;
        public Visibility WaitVisibility { get; } = Visibility.Collapsed;

        public MainViewModel()
        {
            var boardService = new BoardService();
            var world = boardService.BuildWorld(new GameSettings());

            for (int y = 0; y < world.Board.Size; y++)
            {
                for (int x = 0; x < world.Board.Size; x++)
                {
                    var field = world.Board.GetField(x, y);
                    BoardCells.Add(new BoardCellViewModel(field, x, y));
                }
            }

            RoundLabel = "Round: 1";
            PlayerLabel = "Player: Player";
            CreditsLabel = "Credits: 0";
            GalacticTicketsLabel = "Galactic tickets: 0";
            PirateDefenseCardLabel = "Pirate defense card: none";
            LogLabel = "Game actions will appear here.";
        }
    }

    public class BoardCellViewModel
    {
        public int X { get; }
        public int Y { get; }
        public string Name { get; }
        public FieldKind Kind { get; }
        public string Detail { get; }
        public string Icon { get; }

        public BoardCellViewModel(Field field, int x, int y)
        {
            X = x;
            Y = y;
            Name = field.Name;
            Kind = field.Kind;
            Detail = field.Kind == FieldKind.Planet && field.SystemId != null
                ? $"System {field.SystemId}"
                : field.Kind.ToString();
            Icon = field.Kind switch
            {
                FieldKind.Planet => "🪐",
                FieldKind.RailStop => "🚆",
                FieldKind.PirateAttack => "🏴‍☠️",
                FieldKind.Singularity => "✨",
                _ => "·"
            };
        }
    }
}
