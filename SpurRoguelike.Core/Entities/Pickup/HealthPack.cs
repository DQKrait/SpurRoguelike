using SpurRoguelike.Core.Views;

namespace SpurRoguelike.Core.Entities
{
    public class HealthPack : Pickup
    {
        public HealthPack(string name)
            : base(name)
        {
        }

        public override bool PickUp(Pawn pawn)
        {
            pawn.TakeDamage(-50, null);
            Destroy();
            return true;
        }

        public HealthPackView CreateView()
        {
            return new HealthPackView(this);
        }
    }
}