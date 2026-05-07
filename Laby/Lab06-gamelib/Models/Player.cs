namespace Lab06_gamelib.Models
{
    public class Player
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsHuman { get; private set; }
        public int Credits { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int TrackIndex { get; set; }
        public int TurnsToSkip { get; set; }
        public int ConsecutiveSkips { get; set; }
        public int GalacticTickets { get; set; }
        public bool HasPirateDefenseCard { get; set; }
        public HashSet<int> OwnedFieldIds { get; private set; }

        public Player(int id, string name, bool isHuman)
        {
            Id = id;
            Name = name;
            IsHuman = isHuman;
            OwnedFieldIds = new HashSet<int>();
        }
    }
}
