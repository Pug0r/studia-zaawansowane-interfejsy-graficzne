using System.Collections.Generic;

namespace Lab06_gamelib.Models
{
    public class PlanetarySystem
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public HashSet<int> PlanetFieldIds { get; private set; }
        public int? OwnerId { get; set; }
        public bool HasShipyard { get; set; }
        public int AsteroidMineLevel { get; set; }

        public PlanetarySystem(int id, string name)
        {
            Id = id;
            Name = name;
            PlanetFieldIds = new HashSet<int>();
        }
    }
}
