namespace Lab06_gamelib.Models
{
    public class Planet : Field
    {
        public bool HasPort { get; set; }
        public int SettlementLevel { get; set; }
        public int MineLevel { get; set; }
        public int FarmLevel { get; set; }

        public Planet(int id, string name, int systemId)
            : base(id, name, FieldKind.Planet, systemId)
        {
        }
    }
}
