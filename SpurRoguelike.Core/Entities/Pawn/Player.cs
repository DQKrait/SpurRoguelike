using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Entities
{
    public class Player : Pawn
    {
        public Player(string name, int attack, int defence, int health, int healthMaximum, IPlayerController playerController, IEventReporter eventReporter)
            : base(name, attack, defence, health, healthMaximum)
        {
            EventReporter = eventReporter;
            this.playerController = playerController;
        }

        public override void Tick()
        {
            base.Tick();

            playerController.MakeTurn(Level.CreateView(), EventReporter).Apply(this);
        }

        public override void PerformAttack(Pawn victim)
        {
            base.PerformAttack(victim);

            var damage = (int) (((double) TotalAttack / victim.TotalDefence) * BaseDamage * (1 - Level.Random.NextDouble() / 10));
            victim.TakeDamage(damage, this);

            if (victim is Monster && victim.IsDestroyed)
            {
                var attackBonus = (int) (((double) victim.Attack / Attack) * BaseUpgrade * (1 + Level.Random.NextDouble() / 4));
                var defenceBonus = (int) (((double) victim.Defence / Defence) * BaseUpgrade * (1 + Level.Random.NextDouble() / 4));

                Upgrade(attackBonus, defenceBonus);
            }
        }

        public IEventReporter EventReporter { get; }

        protected override bool ProcessMove(Location newLocation, Level newLevel)
        {
            if (!base.ProcessMove(newLocation, newLevel))
                return false;

            var destination = newLevel.Field[newLocation];

            if (destination == CellType.Exit)
            {
                Level.Complete();
                return false;
            }

            return true;
        }

        private readonly IPlayerController playerController;

        private const int BaseDamage = 10;
        private const double BaseUpgrade = 1.5;
    }
}