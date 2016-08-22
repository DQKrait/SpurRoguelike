using System.Collections.Generic;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Views
{
    public struct FieldView : IView
    {
        public FieldView(Field field)
        {
            this.field = field;
        }

        public CellType this[Location index] => field?[index] ?? CellType.Empty;

        public int Width => field?.Width ?? 0;

        public int Height => field?.Height ?? 0;
        
        public bool Contains(Location location)
        {
            return field?.Contains(location) ?? false;
        }

        public IEnumerable<Location> GetCellsOfType(CellType type)
        {
            return field?.GetCellsOfType(type);
        }
        public bool HasValue => field != null;

        private readonly Field field;
    }
}