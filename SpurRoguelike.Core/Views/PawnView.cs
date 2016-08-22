using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Views
{
    public struct PawnView : IView
    {
        public PawnView(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public string Name => pawn?.Name;

        public int Attack => pawn?.Attack ?? 0;

        public int Defence => pawn?.Defence ?? 0;

        public int TotalAttack => pawn?.TotalAttack ?? 0;

        public int TotalDefence => pawn?.TotalDefence ?? 0;

        public int Health => pawn?.Health ?? 0;

        public bool TryGetEquippedItem(out ItemView item)
        {
            if (pawn?.EquippedItem != null)
            {
                item = pawn.EquippedItem.CreateView();
                return true;
            }

            item = default(ItemView);
            return false;
        }

        public bool IsDestroyed => pawn?.IsDestroyed ?? false;

        public Location Location => pawn?.Location ?? default(Location);

        public bool HasValue => pawn != null;

        private readonly Pawn pawn;
    }
}