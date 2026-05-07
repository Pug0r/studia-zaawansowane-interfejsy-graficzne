namespace Lab06_gamelib.Models
{
    public class Field
    {
        public int Radius { get; private set; }
        public string Name { get; private set; }
        public int Id { get; private set; }
        public FieldKind Kind { get; private set; }
        public int? SystemId { get; private set; }
        public int? OwnerId { get; set; }

        public Field(int radius, int id, string name, FieldKind kind, int? systemId = null)
        {
            Radius = radius;
            Name = name;
            Id = id;
            Kind = kind;
            SystemId = systemId;
        }
    }
}
