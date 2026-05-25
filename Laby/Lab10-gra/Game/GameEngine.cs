using Lab06_gamelib;
using Lab06_gamelib.Models;
using Lab06_gamelib.Services;
using Lab10_gra.Game.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab10_gra.Game
{
    public class GameEngine
    {
        private readonly BoardService _boardService = new();
        private readonly DiceService _dice = new();
        private readonly SettingsProvider _settingsProvider = new();
        private readonly GameDecisionContext _decisions = new();
        private readonly BoardHelper _boardHelper;
        private readonly PlayerHelper _playerHelper;
        private readonly CardHelper _cardHelper;

        private GameSettings _settings = new();
        private GameState? _state;
        private Player? _currentPlayer;

        public event Action<string>? Log;
        public event Action<GameState>? StateChanged;
        public event Action<Player>? PlayerChanged;
        public event Action<IReadOnlyCollection<DecisionType>>? DecisionsChanged;
        public event Action? GameFinished;

        public GameDecisionContext Decisions => _decisions;
        public GameState? State => _state;

        public GameEngine()
        {
            _settings = _settingsProvider.Settings;
            _boardHelper = new BoardHelper();
            _playerHelper = new PlayerHelper(_dice);
            _cardHelper = new CardHelper(_dice);
        }

        public GameWorld BuildWorld() => _boardService.BuildWorld(_settings);

        public async Task StartAsync(GameWorld world)
        {
            var players = _playerHelper.CreatePlayers(_settings.StartingCredits);
            _state = new GameState(players, world);
            RaiseLog("Game started.");
            RaiseStateChanged();

            for (int round = 1; round <= _settings.RoundLimit; round++)
            {
                foreach (var player in players)
                {
                    await ExecuteTurnAsync(_state, player);
                }

                GameEconomy.ApplyIncomeIfDue(_state, _settings);
                RaiseStateChanged();
            }

            RaiseLog("Game finished.");
            GameFinished?.Invoke();
        }

        private async Task ExecuteTurnAsync(GameState state, Player player)
        {
            _currentPlayer = player;
            PlayerChanged?.Invoke(player);

            if (player.TurnsToSkip > 0)
            {
                player.TurnsToSkip--;
                RaiseLog($"{player.Name} skips a turn.");
                await Task.Delay(200);
                return;
            }

            if (player.IsHuman && player.ConsecutiveSkips < 2)
            {
                var decision = await WaitForDecisionAsync(DecisionType.TakeTurn, DecisionType.SkipTurn);
                if (decision == DecisionType.SkipTurn)
                {
                    player.ConsecutiveSkips++;
                    RaiseLog($"{player.Name} skips by choice.");
                    return;
                }
            }

            player.ConsecutiveSkips = 0;
            int roll = _dice.Roll(1, 6);
            _boardHelper.MovePlayer(state.World, player, roll);

            var field = state.World.Board.GetField(player.X, player.Y);
            RaiseLog($"{player.Name} rolled {roll} and landed on {field.Name} ({field.Kind}).");
            RaiseStateChanged();

            await ResolveFieldAsync(state, player, field);
        }

        private async Task ResolveFieldAsync(GameState state, Player player, Field field)
        {
            switch (field)
            {
                case { Kind: FieldKind.PirateAttack }:
                    await ResolvePirateEncounterAsync(player);
                    return;
                case { Kind: FieldKind.RailStop }:
                    await HandleRailStopAsync(state, player);
                    return;
                case { Kind: FieldKind.Singularity }:
                    await HandleSingularityAsync(state, player);
                    return;
                case Planet planet:
                    await HandlePlanetAsync(state, player, planet);
                    return;
            }
        }

        private async Task HandleRailStopAsync(GameState state, Player player)
        {
            if (player.GalacticTickets <= 0)
            {
                return;
            }

            if (player.IsHuman)
            {
                var decision = await WaitForDecisionAsync(DecisionType.UseRailStop, DecisionType.Wait);
                if (decision != DecisionType.UseRailStop)
                {
                    return;
                }
            }

            if (_boardHelper.TryMoveByRail(state.World, player))
            {
                RaiseLog($"{player.Name} moved by rail.");
                RaiseStateChanged();
            }
        }

        private async Task HandleSingularityAsync(GameState state, Player player)
        {
            var card = _cardHelper.DrawSingularityCard(state, player);
            RaiseLog($"Singularity card: {card}");

            switch (card)
            {
                case SingularityCardKind.PirateAttack:
                    await ResolvePirateEncounterAsync(player);
                    break;
                case SingularityCardKind.PirateDefense:
                    player.HasPirateDefenseCard = true;
                    RaiseLog($"{player.Name} received pirate defense card.");
                    break;
                case SingularityCardKind.GalacticTicket:
                    player.GalacticTickets++;
                    RaiseLog($"{player.Name} received galactic ticket.");
                    break;
                case SingularityCardKind.Tax:
                    int value = GameEconomy.CalculatePropertyValue(state, player, _settings);
                    int tax = value * _settings.TaxRatePercent / 100;
                    int paid = Math.Min(player.Credits, tax);
                    player.Credits -= paid;
                    RaiseLog($"{player.Name} paid tax {paid}.");
                    break;
                case SingularityCardKind.Lottery:
                    player.Credits += _settings.LotteryReward;
                    RaiseLog($"{player.Name} won lottery {_settings.LotteryReward}.");
                    break;
                case SingularityCardKind.EngineFailure:
                    player.TurnsToSkip += _settings.EngineFailureTurnsLost;
                    int failurePaid = Math.Min(player.Credits, _settings.EngineFailureTowCost);
                    player.Credits -= failurePaid;
                    RaiseLog($"{player.Name} had engine failure and paid {failurePaid}.");
                    break;
                case SingularityCardKind.ShipyardFailure:
                    GameWorldQueries.HandleShipyardFailure(state, player, _settings);
                    break;
            }

            RaiseStateChanged();
        }

        private async Task ResolvePirateEncounterAsync(Player player)
        {
            if (player.HasPirateDefenseCard)
            {
                if (player.IsHuman)
                {
                    var decision = await WaitForDecisionAsync(DecisionType.UsePirateDefense, DecisionType.KeepPirateDefense);
                    if (decision == DecisionType.UsePirateDefense)
                    {
                        player.HasPirateDefenseCard = false;
                        RaiseLog($"{player.Name} blocked pirates with defense card.");
                        RaiseStateChanged();
                        return;
                    }
                }
                else
                {
                    player.HasPirateDefenseCard = false;
                    RaiseLog($"{player.Name} blocked pirates with defense card.");
                    RaiseStateChanged();
                    return;
                }
            }

            if (player.Credits >= _settings.PirateRansomCost)
            {
                if (player.IsHuman)
                {
                    var decision = await WaitForDecisionAsync(DecisionType.PayRansom, DecisionType.Wait);
                    if (decision == DecisionType.PayRansom)
                    {
                        player.Credits -= _settings.PirateRansomCost;
                        RaiseLog($"{player.Name} paid ransom.");
                        RaiseStateChanged();
                        return;
                    }
                }
                else
                {
                    player.Credits -= _settings.PirateRansomCost;
                    RaiseLog($"{player.Name} paid ransom.");
                    RaiseStateChanged();
                    return;
                }
            }

            player.TurnsToSkip += _settings.PirateTurnsLost;
            RaiseLog($"{player.Name} loses {_settings.PirateTurnsLost} turns.");
            RaiseStateChanged();
        }

        private async Task HandlePlanetAsync(GameState state, Player player, Planet planet)
        {
            if (!_playerHelper.CanHandlePlanet(player, planet, _settings))
            {
                return;
            }

            if (!planet.HasPort)
            {
                if (player.IsHuman)
                {
                    var decision = await WaitForDecisionAsync(DecisionType.BuildPort, DecisionType.Wait);
                    if (decision != DecisionType.BuildPort)
                    {
                        return;
                    }
                }

                if (_playerHelper.TryBuildPort(player, planet, _settings))
                {
                    GameWorldQueries.UpdateSystemOwnership(state, planet.SystemId);
                    RaiseStateChanged();
                }

                return;
            }

            if (planet.OwnerId != player.Id)
            {
                return;
            }

            var system = GameWorldQueries.FindSystem(state, planet.SystemId);
            if (system != null && system.OwnerId == player.Id)
            {
                if (player.IsHuman)
                {
                    var decision = await WaitForDecisionAsync(
                        DecisionType.UpgradeSettlement,
                        DecisionType.UpgradeMine,
                        DecisionType.UpgradeFarm,
                        DecisionType.BuildShipyard,
                        DecisionType.UpgradeAsteroidMine,
                        DecisionType.Wait);

                    _playerHelper.ApplyUpgradeDecision(player, planet, system, _settings, decision, RaiseLog);
                }
                else
                {
                    _playerHelper.ApplyBotSystemUpgrade(player, planet, system, _settings, RaiseLog);
                }

                RaiseStateChanged();
                return;
            }

            if (player.IsHuman)
            {
                var decision = await WaitForDecisionAsync(
                    DecisionType.UpgradeSettlement,
                    DecisionType.UpgradeMine,
                    DecisionType.UpgradeFarm,
                    DecisionType.Wait);

                _playerHelper.ApplyUpgradeDecision(player, planet, system, _settings, decision, RaiseLog);
            }
            else
            {
                _playerHelper.ApplyBotBasicUpgrade(player, planet, _settings, RaiseLog);
            }

            RaiseStateChanged();
        }

        private async Task<DecisionType> WaitForDecisionAsync(params DecisionType[] decisions)
        {
            DecisionsChanged?.Invoke(decisions);
            var result = await _decisions.WaitForDecisionAsync(decisions);
            DecisionsChanged?.Invoke(Array.Empty<DecisionType>());
            return result;
        }

        private void RaiseLog(string message)
        {
            Log?.Invoke(message);
        }

        private void RaiseStateChanged()
        {
            if (_state != null)
            {
                StateChanged?.Invoke(_state);
            }
        }
    }
}
