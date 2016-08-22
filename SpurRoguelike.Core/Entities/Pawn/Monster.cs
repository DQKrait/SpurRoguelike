namespace SpurRoguelike.Core.Entities
{
    public abstract class Monster : Pawn
    {
        protected Monster(string name, int attack, int defence, int health, int healthMaximum) 
            : base(name, attack, defence, health, healthMaximum)
        {
        }

        public override void PerformAttack(Pawn victim)
        {
            if (victim is Monster)
                return;

            base.PerformAttack(victim);

            var damage = (int)(((double) TotalAttack / victim.TotalDefence) * BaseDamage * (1 - Level.Random.NextDouble() / 10));
            victim.TakeDamage(damage, this);
        }

        private const int BaseDamage = 10;
    }
}