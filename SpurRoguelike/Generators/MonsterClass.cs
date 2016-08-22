using System;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.Generators
{
    internal class MonsterClass
    {
        public MonsterClass(Func<Monster> factory, double rarity, double skill)
        {
            Factory = factory;
            Rarity = rarity;
            Skill = skill;
        }

        public Func<Monster> Factory { get; }

        public double Rarity { get; }

        public double Skill { get; }
    }
}