using System;
using System.Collections.Generic;
using System.Text;

namespace Lab06_gamelib.Models
{
    public class Board
    {
        private readonly Field[,] _grid;
        public int Size { get; }

        public Board(int size)
        {
            Size = size;
            _grid = new Field[size, size];
        }

        public void SetField(int x, int y, Field field)
        {
            _grid[x, y] = field;
        }

        public Field GetField(int x, int y) => _grid[x, y];
    }
}
