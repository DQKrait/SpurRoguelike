using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Views
{
    public class LevelView : IView
    {
        public LevelView(Level level)
        {
            this.level = level;
        }

        public FieldView Field => level?.Field?.CreateView() ?? default(FieldView);

        public PawnView Player => level?.Player?.CreateView() ?? default(PawnView);

        public IEnumerable<PawnView> Monsters => level?.Monsters.Select(m => m.CreateView());

        public IEnumerable<ItemView> Items => level?.Items.Select(i => i.CreateView());

        public IEnumerable<HealthPackView> HealthPacks => level?.HealthPacks.Select(hp => hp.CreateView());

        public Random Random => level?.Random;

        public bool HasValue => level != null;

        public PawnView GetMonsterAt(Location location)
        {
            return level?.GetEntity<Monster>(location)?.CreateView() ?? default(PawnView);
        }

        public ItemView GetItemAt(Location location)
        {
            return level?.GetEntity<Item>(location)?.CreateView() ?? default(ItemView);
        }

        public HealthPackView GetHealthPackAt(Location location)
        {
            return level?.GetEntity<HealthPack>(location)?.CreateView() ?? default(HealthPackView);
        }

        private readonly Level level;
    }
}