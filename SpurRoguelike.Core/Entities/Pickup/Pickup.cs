namespace SpurRoguelike.Core.Entities
{
    public abstract class Pickup : Entity
    {
        protected Pickup(string name)
            : base(name)
        {
        }

        public abstract bool PickUp(Pawn pawn);
    }
}