using System.Collections.Generic;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.Core
{
    public class Field
    {
        public Field(int width, int height)
        {
            cells = new CellType[width, height];
        }

        public FieldView CreateView()
        {
            return new FieldView(this);
        }

        public CellType this[Location index]
        {
            get { return cells[index.X, index.Y]; }
            set
            {
                cells[index.X, index.Y] = value;
                if (value == CellType.PlayerStart)
                    PlayerStart = index;
            }
        }

        public bool Contains(Location location)
        {
            return location.X >= 0 && location.X < Width && location.Y >= 0 && location.Y < Height;
        }

        public IEnumerable<Location> GetCellsOfType(CellType type)
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    if (cells[i, j] == type)
                        yield return new Location(i, j);
        }

        public int Width => cells.GetLength(0);

        public int Height => cells.GetLength(1);

        public Location PlayerStart { get; private set; }

        private readonly CellType[,] cells;
    }
}