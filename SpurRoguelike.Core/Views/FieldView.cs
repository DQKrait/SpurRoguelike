using System.Collections.Generic;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Views
{
    public struct FieldView
    {
        public FieldView(Field field)
        {
            this.field = field;
        }

        public CellType this[Location index] => field[index];

        public int Width => field.Width;

        public int Height => field.Height;
        
        public bool Contains(Location location)
        {
            return field.Contains(location);
        }

        public IEnumerable<Location> GetCellsOfType(CellType type)
        {
            return field.GetCellsOfType(type);
        }

        private readonly Field field;
    }
}