using System;
using System.Linq;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Content
{
    internal class ArenaFighter : Monster
    {
        public ArenaFighter(string name, int attack, int defence, int health, int healthMaximum, double skill)
            : base(name, attack, defence, health, healthMaximum)
        {
            sightRadius = (int)(10 + 20 * skill);
            panicHealthLimit = healthMaximum / 10;
            state = new StateIdle(this);
        }

        public override void Tick()
        {
            base.Tick();

            if (enemy != null && (enemy.IsDestroyed || enemy.Level != Level))
                enemy = null;

            if (objective != null && (objective.IsDestroyed || objective.Level != Level))
                objective = null;

            state.Tick();
        }

        public override void TakeDamage(int damage, Entity instigator)
        {
            if (instigator is Pawn && enemy == null)
                enemy = (Pawn) instigator;

            base.TakeDamage(damage, instigator);
        }

        private double GetFitnessToFightWith(Pawn opponent, out bool needMoreHealth, out bool needMoreAttack, out bool needMoreDefence)
        {
            needMoreHealth = needMoreAttack = needMoreDefence = false;

            if (Health < panicHealthLimit)
            {
                needMoreHealth = true;
                return 0;
            }

            var predictedDamageToOpponent = (int) (((double) TotalAttack / opponent.TotalDefence) * 9);

            if (predictedDamageToOpponent >= opponent.Health)
                return 1;

            var predictedDamageToSelf = (int) (((double) opponent.TotalAttack / TotalDefence) * 10);

            var damageToTake = Math.Ceiling((double) opponent.Health / predictedDamageToOpponent) * predictedDamageToSelf;

            if (damageToTake >= Health)
            {
                if (Health < HealthMaximum)
                {
                    needMoreHealth = true;
                }
                else
                {
                    needMoreAttack = true;
                    needMoreDefence = true;
                }

                return 0.5;
            }

            if (Health < HealthMaximum * 0.6)
                needMoreHealth = true;

            if (damageToTake >= Health / 2)
            {
                needMoreAttack = true;
                needMoreDefence = true;
            }

            return 0.8;
        }

        private double GetItemValue(Item item)
        {
            return item.AttackBonus * 1.2 + item.DefenceBonus;
        }

        private bool TryAvoidObstacle(ref Offset stepDirection)
        {
            var attempts = 0;
            while (attempts < 3)
            {
                if (Level.Field[Location + stepDirection] <= CellType.Trap || Level.GetEntity<Entity>(Location + stepDirection) != null)
                {
                    stepDirection = stepDirection.Turn(1);
                    attempts++;
                }
                else
                    return true;
            }

            return false;
        }

        private readonly int sightRadius;
        private readonly int panicHealthLimit;
        private State<ArenaFighter> state;
        private Pawn enemy;
        private Entity objective;

        private class StateIdle : State<ArenaFighter>
        {
            public StateIdle(ArenaFighter self)
                : base(self)
            {
            }

            public override void Tick()
            {
                if (Self.enemy != null && Self.IsInRange(Self.enemy, Self.sightRadius))
                {
                    GoToState(() => new StateSeeEnemy(Self));
                    return;
                }

                if (Self.objective != null && Self.IsInRange(Self.objective, Self.sightRadius))
                {
                    GoToState(() => new StateSeeObjective(Self));
                    return;
                }

                var seenPawn = Self.Level.Pawns.FirstOrDefault(p => p != Self && Self.IsInRange(p, Self.sightRadius));

                if (seenPawn != null)
                {
                    Self.enemy = seenPawn;
                    GoToState(() => new StateSeeEnemy(Self));
                    return;
                }

                var seenItem = Self.Level.Items.Where(i => Self.IsInRange(i, Self.sightRadius)).OrderByDescending(Self.GetItemValue).FirstOrDefault();

                if (seenItem != null && (Self.EquippedItem == null || Self.GetItemValue(Self.EquippedItem) < Self.GetItemValue(seenItem)))
                {
                    Self.objective = seenItem;
                    GoToState(() => new StateSeeObjective(Self));
                    return;
                }

                if (Self.Health < Self.HealthMaximum * 0.9)
                {
                    var seenHealth = Self.Level.HealthPacks.Where(i => Self.IsInRange(i, Self.sightRadius)).OrderBy(i => (i.Location - Self.Location).Size()).FirstOrDefault();

                    if (seenHealth != null)
                    {
                        Self.objective = seenHealth;
                        GoToState(() => new StateSeeObjective(Self));
                        return;
                    }
                }

                var stepDirection = Self.Level.Random.Select(Offset.StepOffsets);

                if (Self.TryAvoidObstacle(ref stepDirection))
                    Self.Move(Self.Location + stepDirection, Self.Level);
            }

            public override void GoToState<TState>(Func<TState> factory)
            {
                Self.state = factory();
                Self.state.Tick();
            }
        }

        private class StateSeeEnemy : State<ArenaFighter>
        {
            public StateSeeEnemy(ArenaFighter self)
                : base(self)
            {
            }

            public override void Tick()
            {
                if (Self.enemy == null)
                {
                    GoToState(() => new StateIdle(Self));
                    return;
                }

                bool needMoreHealth;
                bool needMoreAttack;
                bool needMoreDefence;
                var fitness = Self.GetFitnessToFightWith(Self.enemy, out needMoreHealth, out needMoreAttack, out needMoreDefence);
                
                if (fitness >= 0.8)
                {
                    if (Self.IsInAttackRange(Self.enemy))
                    {
                        Self.PerformAttack(Self.enemy);
                        return;
                    }

                    var stepDirection = (Self.enemy.Location - Self.Location).SnapToStep(Self.Level.Random);

                    if (Self.TryAvoidObstacle(ref stepDirection))
                        Self.Move(Self.Location + stepDirection, Self.Level);
                }
                else
                {
                    var hasObjective = TrySetObjective(needMoreHealth, needMoreAttack, needMoreDefence);

                    if (hasObjective)
                    {
                        var stepToObjective = Offset.StepOffsets.FirstOrDefault(o => Self.Location + o == Self.objective.Location);

                        if (stepToObjective != default(Offset))
                        {
                            Self.Move(Self.Location + stepToObjective, Self.Level);
                            return;
                        }
                    }

                    if (fitness >= 0.5)
                    {
                        if (Self.IsInAttackRange(Self.enemy))
                        {
                            Self.PerformAttack(Self.enemy);
                            return;
                        }

                        var offsetToTarget = hasObjective ? Self.objective.Location - Self.Location : Self.enemy.Location - Self.Location;

                        var stepDirection = offsetToTarget.SnapToStep(Self.Level.Random);

                        if (Self.TryAvoidObstacle(ref stepDirection))
                            Self.Move(Self.Location + stepDirection, Self.Level);
                    }
                    else
                    {
                        var offsetToTarget = hasObjective ? Self.objective.Location - Self.Location : -(Self.enemy.Location - Self.Location);

                        var stepDirection = offsetToTarget.SnapToStep(Self.Level.Random);

                        if (Self.TryAvoidObstacle(ref stepDirection))
                        {
                            Self.Move(Self.Location + stepDirection, Self.Level);
                            return;
                        }

                        if (Self.IsInAttackRange(Self.enemy))
                            Self.PerformAttack(Self.enemy);
                    }
                }
            }

            private bool TrySetObjective(bool needMoreHealth, bool needMoreAttack, bool needMoreDefence)
            {
                if (needMoreHealth)
                {
                    if (Self.objective is HealthPack)
                        return true;

                    var seenHealth = Self.Level.HealthPacks.Where(i => Self.IsInRange(i, Self.sightRadius)).OrderBy(i => (i.Location - Self.Location).Size()).FirstOrDefault();

                    if (seenHealth != null)
                    {
                        Self.objective = seenHealth;
                        return true;
                    }
                }

                if (needMoreAttack || needMoreDefence)
                {
                    var existingObjective = Self.objective as Item;

                    if (existingObjective != null &&
                        (!needMoreAttack || existingObjective.AttackBonus > Self.EquippedItem?.AttackBonus) &&
                        (!needMoreDefence || existingObjective.DefenceBonus > Self.EquippedItem?.DefenceBonus))
                        return true;

                    var seenItem = Self.Level.Items
                        .Where(i => Self.IsInRange(i, Self.sightRadius))
                        .Where(i => (!needMoreAttack || i.AttackBonus > Self.EquippedItem?.AttackBonus) && (!needMoreDefence || i.DefenceBonus > Self.EquippedItem?.DefenceBonus))
                        .OrderBy(Self.GetItemValue).FirstOrDefault();

                    if (seenItem != null)
                    {
                        Self.objective = seenItem;
                        return true;
                    }
                }

                return false;
            }

            public override void GoToState<TState>(Func<TState> factory)
            {
                Self.state = factory();
                Self.state.Tick();
            }
        }

        private class StateSeeObjective : State<ArenaFighter>
        {
            public StateSeeObjective(ArenaFighter self)
                : base(self)
            {
            }

            public override void Tick()
            {
                if (Self.objective == null)
                {
                    GoToState(() => new StateIdle(Self));
                    return;
                }

                if (Self.enemy != null && Self.IsInRange(Self.enemy, Self.sightRadius))
                {
                    GoToState(() => new StateSeeEnemy(Self));
                    return;
                }

                var stepToObjective = Offset.StepOffsets.FirstOrDefault(o => Self.Location + o == Self.objective.Location);

                if (stepToObjective != default(Offset))
                {
                    Self.Move(Self.Location + stepToObjective, Self.Level);
                    return;
                }

                var offsetToTarget = Self.objective.Location - Self.Location;

                var stepDirection = offsetToTarget.SnapToStep(Self.Level.Random);

                if (Self.TryAvoidObstacle(ref stepDirection))
                    Self.Move(Self.Location + stepDirection, Self.Level);
            }

            public override void GoToState<TState>(Func<TState> factory)
            {
                Self.state = factory();
                Self.state.Tick();
            }
        }
    }
}