using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Views
{
    public struct HealthPackView : IView
    {
        public HealthPackView(HealthPack healthPack)
        {
            this.healthPack = healthPack;
        }

        public Location Location => healthPack?.Location ?? default(Location);
        
        public bool HasValue => healthPack != null;

        private readonly HealthPack healthPack;
    }
}