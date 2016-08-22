using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.Core.Entities
{
    public class Item : Pickup
    {
        public Item(string name, int attackBonus, int defenceBonus)
            : base(name)
        {
            AttackBonus = attackBonus;
            DefenceBonus = defenceBonus;
        }

        public override bool PickUp(Pawn newOwner)
        {
            if (Owner != null)
                return false;

            newOwner.Equip(this);

            Owner = newOwner;
            Destroy();
            return true;
        }

        public void Drop()
        {
            if (Owner == null)
                return;

            foreach (var offset in Offset.AttackOffsets)
            {
                var freeCell = Owner.Location + offset;

                if (!Owner.Level.Field.Contains(freeCell) || Owner.Level.Field[freeCell] < CellType.PlayerStart)
                    continue;

                if (Owner.Level.GetEntity<Entity>(freeCell) != null)
                    continue;

                var newLevel = Owner.Level;
                Owner = null;

                newLevel.Spawn(freeCell, this);
                return;
            }
        }

        protected override bool ProcessMove(Location newLocation, Level newLevel)
        {
            if (Owner != null)
                return false;

            return true;
        }

        public ItemView CreateView()
        {
            return new ItemView(this);
        }

        public Pawn Owner { get; private set; }

        public bool IsEquipped => Owner != null;

        public int AttackBonus { get; }

        public int DefenceBonus { get; }
    }
}