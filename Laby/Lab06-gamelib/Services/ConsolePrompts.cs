using System;

namespace Lab06_gamelib.Services
{
    public static class ConsolePrompts
    {
        public const string TurnAction = "Choose action: 1-roll, 2-skip";
        public const string RailStopIndex = "Rail stop index";
        public const string UsePirateDefense = "Use pirate defense card? 1-yes 2-no";
        public const string BuildPort = "Build port? 1-yes 2-no";
        public const string UpgradeWithSystem = "Choose upgrade: 1-settlement, 2-mine, 3-farm, 4-shipyard, 5-asteroid mine";
        public const string UpgradeBasic = "Choose upgrade: 1-settlement, 2-mine, 3-farm";

        public static string PayRansom(int cost) => $"Pay ransom {cost}? 1-yes 2-no";

        public static int? AskChoice(string prompt, int min, int max, bool allowEmpty)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return allowEmpty ? null : 0;
                }

                if (int.TryParse(input.Trim(), out int value) && value >= min && value <= max)
                {
                    return value;
                }
            }
        }
    }
}
