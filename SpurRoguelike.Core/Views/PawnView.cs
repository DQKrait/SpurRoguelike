using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Views
{
    public struct PawnView
    {
        public PawnView(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public string Name => pawn.Name;

        public int Attack => pawn.Attack;

        public int Defence => pawn.Defence;

        public int Health => pawn.Health;

        public bool TryGetEquippedItem(out ItemView item)
        {
            if (pawn.EquippedItem != null)
            {
                item = pawn.EquippedItem.CreateView();
                return true;
            }

            item = default(ItemView);
            return false;
        }

        public bool IsDestroyed => pawn.IsDestroyed;

        public Location Location => pawn.Location;

        public bool HasValue => pawn != null;

        private readonly Pawn pawn;
    }
}