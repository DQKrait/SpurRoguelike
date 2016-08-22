using System;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.Generators
{
    internal class ItemClass
    {
        public ItemClass(Func<Item> factory, double rarity, int level)
        {
            Factory = factory;
            Rarity = rarity;
            Level = level;
        }

        public Func<Item> Factory { get; }

        public double Rarity { get; }

        public int Level { get; set; }
    }
}