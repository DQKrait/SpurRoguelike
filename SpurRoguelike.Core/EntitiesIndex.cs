using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core
{
    internal class EntitiesIndex
    {
        public EntitiesIndex(int width, int height)
        {
            cells = new Entity[width, height];
        }

        public Entity this[Location index]
        {
            get { return cells[index.X, index.Y]; }
            set
            {
                cells[index.X, index.Y] = value;
            }
        }

        public bool Contains(Location location)
        {
            return location.X >= 0 && location.X < Width && location.Y >= 0 && location.Y < Height;
        }

        public void Move(Location from, Location to)
        {
            if (!Contains(from) || !Contains(to))
                return;

            this[to] = this[from];
            this[from] = null;
        }

        public int Width => cells.GetLength(0);

        public int Height => cells.GetLength(1);

        private readonly Entity[,] cells;
    }
}