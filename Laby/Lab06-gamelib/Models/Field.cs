using System;
using System.Collections.Generic;
using System.Text;

namespace Lab06_gamelib.Models
{
    public class Field
    {
        public int Radius { get; private set; }
        public string Name { get; private set; }
        public int Id { get; private set; }

        public Field(int radius, int id, string name)
        {

            Radius = radius;
            Name = name;
            Id = id;
        }

    }
}
