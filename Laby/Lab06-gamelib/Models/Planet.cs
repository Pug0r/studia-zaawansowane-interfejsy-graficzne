namespace Lab06_gamelib.Models
{
    public class Planet : Field
    {
        public int Richness { get; private set; }
        public bool HasPort { get; set; }
        public int SettlementLevel { get; set; }
        public int MineLevel { get; set; }
        public int FarmLevel { get; set; }

        public Planet(int radius, int id, string name, int richness, int systemId)
            : base(radius, id, name, FieldKind.Planet, systemId)
        {
            Richness = richness;
        }
    }
}
