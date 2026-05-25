using Lab06_gamelib.Models;
using Lab06_gamelib.Services;
using System.Collections.Generic;

namespace Lab10_gra.Game.Helpers
{
    public class CardHelper(DiceService dice)
    {
        public SingularityCardKind DrawSingularityCard(GameState state, Player player)
        {
            var cards = new List<SingularityCardKind>
            {
                SingularityCardKind.PirateAttack,
                SingularityCardKind.PirateDefense,
                SingularityCardKind.GalacticTicket,
                SingularityCardKind.Tax,
                SingularityCardKind.Lottery,
                SingularityCardKind.EngineFailure
            };

            if (GameWorldQueries.PlayerHasShipyard(state, player))
            {
                cards.Add(SingularityCardKind.ShipyardFailure);
            }

            int index = dice.Roll(0, cards.Count - 1);
            return cards[index];
        }
    }
}
