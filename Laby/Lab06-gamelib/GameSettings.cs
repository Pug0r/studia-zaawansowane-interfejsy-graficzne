using System.Collections.Generic;

namespace Lab06_gamelib
{
    public class GameSettings
    {
        public int StartingCredits { get; set; }
        public int RoundLimit { get; set; }
        public int IncomeCadence { get; set; }
        public int IncomePerPort { get; set; }
        public int IncomePerSettlementLevel { get; set; }
        public int IncomePerMineLevel { get; set; }
        public int IncomePerFarmLevel { get; set; }
        public int IncomePerShipyard { get; set; }
        public int IncomePerAsteroidMineLevel { get; set; }
        public int PortCost { get; set; }
        public List<int> SettlementUpgradeCosts { get; set; } = new();
        public List<int> MineUpgradeCosts { get; set; } = new();
        public List<int> FarmUpgradeCosts { get; set; } = new();
        public int ShipyardCost { get; set; }
        public List<int> AsteroidMineUpgradeCosts { get; set; } = new();
        public int PirateTurnsLost { get; set; }
        public int PirateRansomCost { get; set; }
        public int TaxRatePercent { get; set; }
        public int LotteryReward { get; set; }
        public int EngineFailureTurnsLost { get; set; }
        public int EngineFailureTowCost { get; set; }
        public int ShipyardFailureCost { get; set; }
    }
}
