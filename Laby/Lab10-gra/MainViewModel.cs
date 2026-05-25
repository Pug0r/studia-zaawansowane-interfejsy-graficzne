using Lab06_gamelib;
using Lab06_gamelib.Models;
using Lab06_gamelib.Services;
using Lab10_gra.Game;
using Lab10_gra.Game.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lab10_gra
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly GameEngine _engine = new();
        private readonly StringBuilder _logBuilder = new();

        public ObservableCollection<BoardCellViewModel> BoardCells { get; } = new();
        public ICommand TakeTurnCommand { get; }
        public ICommand SkipTurnCommand { get; }
        public ICommand PayRansomCommand { get; }
        public ICommand WaitCommand { get; }
        public ICommand UsePirateDefenseCommand { get; }
        public ICommand KeepPirateDefenseCommand { get; }
        public ICommand UseRailStopCommand { get; }
        public ICommand BuildPortCommand { get; }
        public ICommand UpgradeSettlementCommand { get; }
        public ICommand UpgradeMineCommand { get; }
        public ICommand UpgradeFarmCommand { get; }
        public ICommand BuildShipyardCommand { get; }
        public ICommand UpgradeAsteroidMineCommand { get; }

        private string _roundLabel = "Round: 1";
        private string _playerLabel = "Player: Player";
        private string _creditsLabel = "Credits: 0";
        private string _galacticTicketsLabel = "Galactic tickets: 0";
        private string _pirateDefenseCardLabel = "Pirate defense card: none";
        private string _logLabel = "Game actions will appear here.";

        public string RoundLabel { get => _roundLabel; private set => SetProperty(ref _roundLabel, value, nameof(RoundLabel)); }
        public string PlayerLabel { get => _playerLabel; private set => SetProperty(ref _playerLabel, value, nameof(PlayerLabel)); }
        public string CreditsLabel { get => _creditsLabel; private set => SetProperty(ref _creditsLabel, value, nameof(CreditsLabel)); }
        public string GalacticTicketsLabel { get => _galacticTicketsLabel; private set => SetProperty(ref _galacticTicketsLabel, value, nameof(GalacticTicketsLabel)); }
        public string PirateDefenseCardLabel { get => _pirateDefenseCardLabel; private set => SetProperty(ref _pirateDefenseCardLabel, value, nameof(PirateDefenseCardLabel)); }
        public string LogLabel { get => _logLabel; private set => SetProperty(ref _logLabel, value, nameof(LogLabel)); }

        private Visibility _takeTurnVisibility = Visibility.Visible;
        private Visibility _skipTurnVisibility = Visibility.Visible;
        private Visibility _payRansomVisibility = Visibility.Collapsed;
        private Visibility _usePirateDefenseVisibility = Visibility.Collapsed;
        private Visibility _declinePirateDefenseVisibility = Visibility.Collapsed;
        private Visibility _railStopVisibility = Visibility.Collapsed;
        private Visibility _buildPortVisibility = Visibility.Collapsed;
        private Visibility _upgradeSettlementVisibility = Visibility.Collapsed;
        private Visibility _upgradeMineVisibility = Visibility.Collapsed;
        private Visibility _upgradeFarmVisibility = Visibility.Collapsed;
        private Visibility _buildShipyardVisibility = Visibility.Collapsed;
        private Visibility _upgradeAsteroidMineVisibility = Visibility.Collapsed;
        private Visibility _waitVisibility = Visibility.Collapsed;

        public Visibility TakeTurnVisibility { get => _takeTurnVisibility; private set => SetProperty(ref _takeTurnVisibility, value, nameof(TakeTurnVisibility)); }
        public Visibility SkipTurnVisibility { get => _skipTurnVisibility; private set => SetProperty(ref _skipTurnVisibility, value, nameof(SkipTurnVisibility)); }
        public Visibility PayRansomVisibility { get => _payRansomVisibility; private set => SetProperty(ref _payRansomVisibility, value, nameof(PayRansomVisibility)); }
        public Visibility UsePirateDefenseVisibility { get => _usePirateDefenseVisibility; private set => SetProperty(ref _usePirateDefenseVisibility, value, nameof(UsePirateDefenseVisibility)); }
        public Visibility DeclinePirateDefenseVisibility { get => _declinePirateDefenseVisibility; private set => SetProperty(ref _declinePirateDefenseVisibility, value, nameof(DeclinePirateDefenseVisibility)); }
        public Visibility RailStopVisibility { get => _railStopVisibility; private set => SetProperty(ref _railStopVisibility, value, nameof(RailStopVisibility)); }
        public Visibility BuildPortVisibility { get => _buildPortVisibility; private set => SetProperty(ref _buildPortVisibility, value, nameof(BuildPortVisibility)); }
        public Visibility UpgradeSettlementVisibility { get => _upgradeSettlementVisibility; private set => SetProperty(ref _upgradeSettlementVisibility, value, nameof(UpgradeSettlementVisibility)); }
        public Visibility UpgradeMineVisibility { get => _upgradeMineVisibility; private set => SetProperty(ref _upgradeMineVisibility, value, nameof(UpgradeMineVisibility)); }
        public Visibility UpgradeFarmVisibility { get => _upgradeFarmVisibility; private set => SetProperty(ref _upgradeFarmVisibility, value, nameof(UpgradeFarmVisibility)); }
        public Visibility BuildShipyardVisibility { get => _buildShipyardVisibility; private set => SetProperty(ref _buildShipyardVisibility, value, nameof(BuildShipyardVisibility)); }
        public Visibility UpgradeAsteroidMineVisibility { get => _upgradeAsteroidMineVisibility; private set => SetProperty(ref _upgradeAsteroidMineVisibility, value, nameof(UpgradeAsteroidMineVisibility)); }
        public Visibility WaitVisibility { get => _waitVisibility; private set => SetProperty(ref _waitVisibility, value, nameof(WaitVisibility)); }

        public MainViewModel()
        {
            TakeTurnCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.TakeTurn));
            SkipTurnCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.SkipTurn));
            PayRansomCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.PayRansom));
            WaitCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.Wait));
            UsePirateDefenseCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.UsePirateDefense));
            KeepPirateDefenseCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.KeepPirateDefense));
            UseRailStopCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.UseRailStop));
            BuildPortCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.BuildPort));
            UpgradeSettlementCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.UpgradeSettlement));
            UpgradeMineCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.UpgradeMine));
            UpgradeFarmCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.UpgradeFarm));
            BuildShipyardCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.BuildShipyard));
            UpgradeAsteroidMineCommand = new RelayCommand(() => _engine.Decisions.TryCompleteDecision(DecisionType.UpgradeAsteroidMine));

            var world = _engine.BuildWorld();

            for (int y = 0; y < world.Board.Size; y++)
            {
                for (int x = 0; x < world.Board.Size; x++)
                {
                    var field = world.Board.GetField(x, y);
                    BoardCells.Add(new BoardCellViewModel(field, x, y));
                }
            }

            _engine.Log += AppendLog;
            _engine.StateChanged += UpdateState;
            _engine.PlayerChanged += UpdatePlayerInfo;
            _engine.DecisionsChanged += UpdateDecisions;
            _engine.GameFinished += () => UpdateDecisions(Array.Empty<DecisionType>());
            _ = _engine.StartAsync(world);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void UpdatePlayerInfo(Player player)
        {
            PlayerLabel = $"Player: {player.Name}";
            CreditsLabel = $"Credits: {player.Credits}";
            GalacticTicketsLabel = $"Galactic tickets: {player.GalacticTickets}";
            PirateDefenseCardLabel = player.HasPirateDefenseCard ? "Pirate defense card: yes" : "Pirate defense card: none";
        }

        private void UpdateState(GameState state)
        {
            foreach (var cell in BoardCells)
            {
                var field = state.World.Board.GetField(cell.X, cell.Y);
                cell.UpdateFromField(field);
            }
        }

        private void AppendLog(string message)
        {
            if (_logBuilder.Length == 0)
            {
                _logBuilder.Append(message);
            }
            else
            {
                _logBuilder.AppendLine();
                _logBuilder.Append(message);
            }

            LogLabel = _logBuilder.ToString();
        }

        private void UpdateDecisions(IReadOnlyCollection<DecisionType> decisions)
        {
            TakeTurnVisibility = GetVisibility(decisions, DecisionType.TakeTurn, defaultVisible: true);
            SkipTurnVisibility = GetVisibility(decisions, DecisionType.SkipTurn, defaultVisible: true);
            PayRansomVisibility = GetVisibility(decisions, DecisionType.PayRansom);
            WaitVisibility = GetVisibility(decisions, DecisionType.Wait);
            UsePirateDefenseVisibility = GetVisibility(decisions, DecisionType.UsePirateDefense);
            DeclinePirateDefenseVisibility = GetVisibility(decisions, DecisionType.KeepPirateDefense);
            RailStopVisibility = GetVisibility(decisions, DecisionType.UseRailStop);
            BuildPortVisibility = GetVisibility(decisions, DecisionType.BuildPort);
            UpgradeSettlementVisibility = GetVisibility(decisions, DecisionType.UpgradeSettlement);
            UpgradeMineVisibility = GetVisibility(decisions, DecisionType.UpgradeMine);
            UpgradeFarmVisibility = GetVisibility(decisions, DecisionType.UpgradeFarm);
            BuildShipyardVisibility = GetVisibility(decisions, DecisionType.BuildShipyard);
            UpgradeAsteroidMineVisibility = GetVisibility(decisions, DecisionType.UpgradeAsteroidMine);
        }

        private static Visibility GetVisibility(IReadOnlyCollection<DecisionType> decisions, DecisionType decision, bool defaultVisible = false)
        {
            return decisions.Contains(decision) || (defaultVisible && decisions.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return;
            }

            field = value;
            OnPropertyChanged(propertyName ?? string.Empty);
        }
    }

    public class BoardCellViewModel : INotifyPropertyChanged
    {
        public int X { get; }
        public int Y { get; }
        private string _name;
        private FieldKind _kind;
        private string _detail;
        private string _icon;

        public string Name { get => _name; private set => SetProperty(ref _name, value, nameof(Name)); }
        public FieldKind Kind { get => _kind; private set => SetProperty(ref _kind, value, nameof(Kind)); }
        public string Detail { get => _detail; private set => SetProperty(ref _detail, value, nameof(Detail)); }
        public string Icon { get => _icon; private set => SetProperty(ref _icon, value, nameof(Icon)); }

        public BoardCellViewModel(Field field, int x, int y)
        {
            X = x;
            Y = y;
            UpdateFromField(field);
        }

        public void UpdateFromField(Field field)
        {
            Name = field.Name;
            Kind = field.Kind;
            Detail = BuildDetail(field);
            Icon = GetIcon(field.Kind);
        }

        private static string BuildDetail(Field field)
        {
            if (field is Planet planet)
            {
                string owner = planet.OwnerId == null ? "Owner: none" : $"Owner: {planet.OwnerId}";
                string port = planet.HasPort ? "Port: yes" : "Port: no";
                return $"System {planet.SystemId} | {owner} | {port} | S:{planet.SettlementLevel} M:{planet.MineLevel} F:{planet.FarmLevel}";
            }

            return field.SystemId != null ? $"System {field.SystemId}" : field.Kind.ToString();
        }

        private static string GetIcon(FieldKind kind)
        {
            return kind switch
            {
                FieldKind.Planet => "\U0001FA90",
                FieldKind.RailStop => "\U0001F686",
                FieldKind.PirateAttack => "\U0001F3F4\u200D\u2620\uFE0F",
                FieldKind.Singularity => "\u2728",
                _ => "\u00B7"
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return;
            }

            field = value;
            OnPropertyChanged(propertyName);
        }
    }

    public sealed class RelayCommand : ICommand
    {
        private readonly Action _execute;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => _execute();
    }
}
