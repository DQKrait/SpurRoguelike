using System;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.Generators
{
    public class MonsterClassOptions
    {
        public double Skill { get; set; }
        public Func<string, double, int, int, int, Monster> Factory { get; set; }
        public double Rarity { get; set; }
    }
}