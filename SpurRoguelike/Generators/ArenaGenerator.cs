using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Content;
using SpurRoguelike.Core;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Generators
{
    internal class ArenaGenerator : LevelGenerator
    {
        public ArenaGenerator(int seed, NameGenerator nameGenerator)
            : base(seed, nameGenerator)
        {
        }

        protected override void CarveDungeon(Field field, LevelGenerationSettings settings)
        {
            base.CarveDungeon(field, settings);

            var exitLocation = field.GetCellsOfType(CellType.Exit).FirstOrDefault();

            if (exitLocation == default(Location))
                return;

            foreach (var offset in Offset.AttackOffsets)
                field[exitLocation + offset] = CellType.Wall;
        }

        protected override void PopulateWithMonsters(Level level, LevelGenerationSettings settings, IList<MonsterClass> monsterClasses)
        {
            var allFreeCells = level.Field.GetCellsOfType(CellType.Empty);
            var farthestFromPlayer = allFreeCells.OrderByDescending(location => (location - level.Field.PlayerStart).Size()).First();

            level.Spawn(farthestFromPlayer, monsterClasses.Single().Factory());
        }

        public override Level Generate(LevelGenerationSettings settings, IList<MonsterClass> monsterClasses, IList<ItemClass> itemClasses)
        {
            var level = base.Generate(settings, monsterClasses, itemClasses);

            level.Spawn(new Location(), new OpenExitTrigger("OpenExit", int.MaxValue));

            return level;
        }
    }
}