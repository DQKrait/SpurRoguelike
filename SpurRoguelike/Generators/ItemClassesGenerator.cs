using System;
using System.Collections.Generic;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.Generators
{
    internal class ItemClassesGenerator
    {
        public ItemClassesGenerator(int seed, NameGenerator nameGenerator)
        {
            this.nameGenerator = nameGenerator;

            random = new Random(seed);
        }

        public List<ItemClass> Generate(int variance, params ItemClassOptions[] itemClassOptions)
        {
            var itemClasses = new List<ItemClass>();

            foreach (var options in itemClassOptions)
            {
                var classesToGenerate = Math.Max(1, (int)Math.Round(Math.Sqrt(random.NextDouble()) * variance));

                for (int i = 0; i < classesToGenerate; i++)
                {
                    var name = nameGenerator.Generate();
                    int attackBonus;
                    int defenceBonus;
                    GenerateItemStats(options.Level, out attackBonus, out defenceBonus);

                    itemClasses.Add(new ItemClass(() => new Item(name, attackBonus, defenceBonus), options.Rarity, options.Level));
                }
            }

            return itemClasses;
        }
        
        private void GenerateItemStats(int level, out int attackBonus, out int defenceBonus)
        {
            var bonus = random.Next(1, level + 1);

            attackBonus = (int) (random.NextDouble() * bonus);
            defenceBonus = bonus - attackBonus;
        }
        
        private readonly NameGenerator nameGenerator;
        private readonly Random random;
    }
}