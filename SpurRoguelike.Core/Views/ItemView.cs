using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Views
{
    public struct ItemView
    {
        public ItemView(Item item)
        {
            this.item = item;
        }

        public int AttackBonus => item?.AttackBonus ?? 0;

        public int DefenceBonus => item?.DefenceBonus ?? 0;

        public Location Location => item.Location;

        public bool HasValue => item != null;

        private readonly Item item;
    }
}