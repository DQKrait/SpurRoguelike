using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Generators
{
    internal class LevelGenerator
    {
        public LevelGenerator(int seed, NameGenerator nameGenerator)
        {
            Seed = seed;
            NameGenerator = nameGenerator;
            Random = new Random(seed);
        }

        public virtual Level Generate(LevelGenerationSettings settings, IList<MonsterClass> monsterClasses, IList<ItemClass> itemClasses)
        {
            var field = new Field(
                Random.Next(settings.Field.MinWidth, settings.Field.MaxWidth),
                Random.Next(settings.Field.MinHeight, settings.Field.MaxHeight));

            SetPlayerStart(field);

            CarveDungeon(field, settings);

            SetupTraps(field, settings);

            var level = new Level(field, Seed);

            PopulateWithMonsters(level, settings, monsterClasses.Where(c => c.Skill >= settings.Monsters.MinSkill && c.Skill <= settings.Monsters.MaxSkill).ToList());

            PopulateWithItems(level, settings, itemClasses.Where(c => c.Level >= settings.Items.MinLevel && c.Level <= settings.Items.MaxLevel).ToList());

            PopulateWithHealthPacks(level, settings);

            return level;
        }

        protected virtual void SetPlayerStart(Field field)
        {
            var placeAtLeft = Random.NextDouble() <= 0.5;
            var placeAtTop = Random.NextDouble() <= 0.5;

            var xOffset = Random.Next(2, Math.Min(5, field.Width));
            var yOffset = Random.Next(2, Math.Min(5, field.Height));

            var location = new Location(
                placeAtLeft ? xOffset : field.Width - xOffset - 1, 
                placeAtTop ? yOffset : field.Height - yOffset - 1);

            field[location] = CellType.PlayerStart;
        }

        protected virtual void CarveDungeon(Field field, LevelGenerationSettings settings)
        {
            var totalCells = field.Width * field.Height;
            var emptyCells = 1;

            var head = field.PlayerStart;
            var farthestFromStart = head;
            
            while ((double) emptyCells / totalCells < settings.Field.FreeSpaceShare)
            {
                var stepDirection = Random.Select(Offset.StepOffsets);

                var newHead = head + stepDirection;
                if (newHead.X == 1 || newHead.X == field.Width - 2 || newHead.Y == 1 || newHead.Y == field.Height - 2)
                    continue;
                
                if (field[head] == CellType.Wall)
                {
                    field[head] = CellType.Empty;
                    emptyCells++;
                }

                foreach (var offset in Offset.StepOffsets)
                {
                    if (field[head + offset] == CellType.Wall)
                    {
                        field[head + offset] = CellType.Empty;
                        emptyCells++;
                    }
                }

                head = newHead;

                var distanceFromStart = head - field.PlayerStart;
                var farthestDistanceFromStart = farthestFromStart - field.PlayerStart;
                if (Math.Abs(distanceFromStart.XOffset) + Math.Abs(distanceFromStart.YOffset) >
                    Math.Abs(farthestDistanceFromStart.XOffset) + Math.Abs(farthestDistanceFromStart.YOffset))
                    farthestFromStart = head;
            }

            field[farthestFromStart] = CellType.Exit;
        }

        protected virtual void SetupTraps(Field field, LevelGenerationSettings settings)
        {
            var totalFreeCells = field.GetCellsOfType(CellType.Empty).Count();
            var traps = 0;

            var exits = field.GetCellsOfType(CellType.Exit).ToList();

            while ((double) traps / totalFreeCells < settings.Traps.Density)
            {
                var location = new Location(Random.Next(1, field.Width - 1), Random.Next(1, field.Height - 1));

                if (field[location] != CellType.Empty)
                    continue;

                if (exits.Any(e => (e - location).Size() < 3) || (field.PlayerStart - location).Size() < 3)
                    continue;

                field[location] = CellType.Trap;
                traps++;
            }
        }

        protected virtual void PopulateWithMonsters(Level level, LevelGenerationSettings settings, IList<MonsterClass> monsterClasses)
        {
            var totalFreeCells = level.Field.GetCellsOfType(CellType.Empty).Count();
            var cellsWithMonsters = new HashSet<Location>();
            
            while ((double)cellsWithMonsters.Count / totalFreeCells < settings.Monsters.Density)
            {
                var location = new Location(Random.Next(1, level.Field.Width - 1), Random.Next(1, level.Field.Height - 1));

                if (level.Field[location] != CellType.Empty)
                    continue;

                if (!cellsWithMonsters.Add(location))
                    continue;
                
                var monster = ChooseClass(monsterClasses, c => c.Rarity).Factory();

                level.Spawn(location, monster);
            }
        }

        protected virtual void PopulateWithItems(Level level, LevelGenerationSettings settings, IList<ItemClass> itemClasses)
        {
            var totalFreeCells = level.Field.GetCellsOfType(CellType.Empty).Count();
            var cellsWithItems = new HashSet<Location>();

            while ((double)cellsWithItems.Count / totalFreeCells < settings.Items.Density)
            {
                var location = new Location(Random.Next(1, level.Field.Width - 1), Random.Next(1, level.Field.Height - 1));

                if (level.Field[location] != CellType.Empty || level.GetEntity<Entity>(location) != null)
                    continue;

                if (!cellsWithItems.Add(location))
                    continue;

                cellsWithItems.Add(location);
                
                var item = ChooseClass(itemClasses, c => c.Rarity).Factory();

                level.Spawn(location, item);
            }
        }

        protected T ChooseClass<T>(IList<T> classes, Func<T, double> rarityFunc)
        {
            while (true)
            {
                var randomDouble = Random.NextDouble();
                var matchingClasses = classes.Where(c => randomDouble < rarityFunc(c)).ToList();

                if (matchingClasses.Count == 0)
                    continue;

                return Random.Select(matchingClasses);
            }
        }

        protected virtual void PopulateWithHealthPacks(Level level, LevelGenerationSettings settings)
        {
            var totalFreeCells = level.Field.GetCellsOfType(CellType.Empty).Count();
            var cellsWithHealthPacks = new HashSet<Location>();

            while ((double)cellsWithHealthPacks.Count / totalFreeCells < settings.HealthPacks.Density)
            {
                var location = new Location(Random.Next(1, level.Field.Width - 1), Random.Next(1, level.Field.Height - 1));

                if (level.Field[location] != CellType.Empty || level.GetEntity<Entity>(location) != null)
                    continue;

                cellsWithHealthPacks.Add(location);

                var healthPack = new HealthPack("Healing fruit");

                level.Spawn(location, healthPack);
            }
        }

        protected readonly Random Random;
        protected readonly int Seed;
        protected readonly NameGenerator NameGenerator;
    }
}