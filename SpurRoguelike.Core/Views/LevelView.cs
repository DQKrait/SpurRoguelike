using System;
using System.Collections.Generic;
using System.Linq;

namespace SpurRoguelike.Core.Views
{
    public class LevelView
    {
        public LevelView(Level level)
        {
            this.level = level;
        }

        public FieldView Field => level.Field.CreateView();

        public PawnView Player => level.Player.CreateView();

        public IEnumerable<PawnView> Monsters => level.Monsters.Select(m => m.CreateView());

        public IEnumerable<ItemView> Items => level.Items.Select(i => i.CreateView());

        public IEnumerable<HealthPackView> HealthPacks => level.HealthPacks.Select(hp => hp.CreateView());

        public Random Random => level.Random;

        private readonly Level level;
    }
}