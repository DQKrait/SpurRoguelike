using System.Linq;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Content
{
    internal class OpenExitTrigger : Trigger
    {
        public OpenExitTrigger(string name, int cooldown)
            : base(name, cooldown)
        {
        }

        protected override bool CheckCondition()
        {
            return !Level.Monsters.Any();
        }

        protected override void Fire()
        {
            var exitLocation = Level.Field.GetCellsOfType(CellType.Exit).FirstOrDefault();

            if (exitLocation == default(Location))
                return;

            foreach (var offset in Offset.AttackOffsets)
                Level.Field[exitLocation + offset] = CellType.Empty;
        }
    }
}