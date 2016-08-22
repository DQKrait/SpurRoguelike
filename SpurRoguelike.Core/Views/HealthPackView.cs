using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Views
{
    public struct HealthPackView
    {
        public HealthPackView(HealthPack healthPack)
        {
            this.healthPack = healthPack;
        }

        public Location Location => healthPack.Location;
        
        public bool HasValue => healthPack != null;

        private readonly HealthPack healthPack;
    }
}