namespace SpurRoguelike.Core.Entities
{
    public abstract class Trigger : Entity
    {
        protected Trigger(string name, int cooldown)
            : base(name)
        {
            this.cooldown = cooldown;
        }

        public override void Tick()
        {
            base.Tick();

            if (ticksToCool > 0)
            {
                ticksToCool--;
                return;
            }

            if (CheckCondition())
            {
                ticksToCool = cooldown;
                Fire();
            }
        }

        protected abstract bool CheckCondition();

        protected abstract void Fire();
        
        private readonly int cooldown;

        private int ticksToCool;
    }
}