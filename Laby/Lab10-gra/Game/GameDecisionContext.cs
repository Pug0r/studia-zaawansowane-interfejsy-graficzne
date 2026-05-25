using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab10_gra.Game
{
    public class GameDecisionContext
    {
        private TaskCompletionSource<DecisionType>? _decisionSource;
        private HashSet<DecisionType> _allowed = new();

        public IReadOnlyCollection<DecisionType> Allowed => _allowed;

        public Task<DecisionType> WaitForDecisionAsync(params DecisionType[] decisions)
        {
            _allowed = new HashSet<DecisionType>(decisions);
            _decisionSource = new TaskCompletionSource<DecisionType>();
            return _decisionSource.Task;
        }

        public bool TryCompleteDecision(DecisionType decision)
        {
            if (_decisionSource == null || !_allowed.Contains(decision))
            {
                return false;
            }

            _decisionSource.TrySetResult(decision);
            _decisionSource = null;
            _allowed = new HashSet<DecisionType>();
            return true;
        }
    }
}
