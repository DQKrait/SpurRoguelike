namespace SpurRoguelike.Generators
{
    internal class LevelGenerationSettings
    {
        public FieldOptions Field { get; set; }

        public TrapOptions Traps { get; set; }

        public ItemOptions Items { get; set; }

        public HealthPackOptions HealthPacks { get; set; }

        public MonsterOptions Monsters { get; set; }

        public class FieldOptions
        {
            public int MinWidth { get; set; }
            public int MaxWidth { get; set; }
            public int MinHeight { get; set; }
            public int MaxHeight { get; set; }
            public double FreeSpaceShare { get; set; }
        }

        public class TrapOptions
        {
            public double Density { get; set; }
        }

        public class ItemOptions
        {
            public double Density { get; set; }
            public int MinLevel { get; set; }
            public int MaxLevel { get; set; }
        }

        public class HealthPackOptions
        {
            public double Density { get; set; }
        }

        public class MonsterOptions
        {
            public double Density { get; set; }
            public double MinSkill { get; set; }
            public double MaxSkill { get; set; }
        }
    }
}