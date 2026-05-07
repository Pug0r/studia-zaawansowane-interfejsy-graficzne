namespace Lab06_gamelib.Services
{

    public class DiceService
    {
        private readonly Random _random = new Random();

        public int Roll(int minInclusive, int maxInclusive)
        {
            return _random.Next(minInclusive, maxInclusive + 1);
        }
    }
}
