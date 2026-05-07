namespace Lab06_gamelib.Models
{
    public class Field
    {
        public string Name { get; private set; }
        public int Id { get; private set; }
        public FieldKind Kind { get; private set; }
        public int? SystemId { get; private set; }
        public int? OwnerId { get; set; }

        public Field(int id, string name, FieldKind kind, int? systemId = null)
        {
            Name = name;
            Id = id;
            Kind = kind;
            SystemId = systemId;
        }
    }
}
