using System;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Content
{
    internal class Reptiloid : Monster
    {
        public Reptiloid(string name, int attack, int defence, int health, int healthMaximum, double skill)
            : base(name, attack, defence, health, healthMaximum)
        {
            sightRadius = (int) (10 + 20 * skill);
            panicHealthLimit = healthMaximum / 10;
            state = new StateIdle(this);
        }

        public override void Tick()
        {
            base.Tick();

            state.Tick();
        }

        private readonly int sightRadius;
        private readonly int panicHealthLimit;
        private State<Reptiloid> state;
        
        private class StateIdle : State<Reptiloid>
        {
            public StateIdle(Reptiloid self)
                : base(self)
            {
            }

            public override void Tick()
            {
                if (Self.Health < Self.panicHealthLimit)
                {
                    GoToState(() => new StateCowering(Self));
                    return;
                }
                
                if (Self.IsInRange(Self.Level.Player, Self.sightRadius))
                {
                    GoToState(() => new StateAttacking(Self, Self.Level.Player));
                    return;
                }

                var stepDirection = Self.Level.Random.Select(Offset.StepOffsets);

                Self.Move(Self.Location + stepDirection, Self.Level);
            }

            public override void GoToState<TState>(Func<TState> factory)
            {
                Self.state = factory();
                Self.state.Tick();
            }
        }

        private class StateAttacking : State<Reptiloid>
        {
            public StateAttacking(Reptiloid self, Pawn target)
                : base(self)
            {
                this.target = target;
            }

            public override void Tick()
            {
                if (target.IsDestroyed || target.Level != Self.Level)
                {
                    GoToState(() => new StateIdle(Self));
                    return;
                }

                if (Self.Health < Self.panicHealthLimit)
                {
                    GoToState(() => new StateFear(Self, target));
                    return;
                }

                if (!Self.IsInRange(target, Self.sightRadius))
                {
                    GoToState(() => new StateIdle(Self));
                    return;
                }

                if (Self.IsInAttackRange(target))
                {
                    Self.PerformAttack(target);
                    return;
                }

                var offsetToTarget = target.Location - Self.Location;

                var stepDirection = offsetToTarget.SnapToStep(Self.Level.Random);

                Self.Move(Self.Location + AvoidWall(stepDirection), Self.Level);
            }

            public override void GoToState<TState>(Func<TState> factory)
            {
                Self.state = factory();
                Self.state.Tick();
            }

            private Offset AvoidWall(Offset stepDirection)
            {
                for (int i = 0; i < 4 && Self.Level.Field[Self.Location + stepDirection] == CellType.Wall; i++)
                    stepDirection = stepDirection.Turn(1);
                
                return stepDirection;
            }

            private readonly Pawn target;
        }

        private class StateCowering : State<Reptiloid>
        {
            public StateCowering(Reptiloid self)
                : base(self)
            {
            }

            public override void Tick()
            {
                foreach (var offset in Offset.AttackOffsets)
                {
                    Pawn other;
                    if ((other = Self.Level.GetEntity<Pawn>(Self.Location + offset)) != null)
                    {
                        Self.PerformAttack(other);
                        return;
                    }
                }
            }

            public override void GoToState<TState>(Func<TState> factory)
            {
                Self.state = factory();
                Self.state.Tick();
            }
        }

        private class StateFear : State<Reptiloid>
        {
            public StateFear(Reptiloid self, Pawn target)
                : base(self)
            {
                this.target = target;
            }

            public override void Tick()
            {
                if (target.IsDestroyed || target.Level != Self.Level)
                {
                    GoToState(() => new StateCowering(Self));
                    return;
                }

                if (!Self.IsInRange(target, Self.sightRadius))
                {
                    GoToState(() => new StateCowering(Self));
                    return;
                }

                if (Self.IsInAttackRange(target))
                {
                    Self.PerformAttack(target);
                    return;
                }

                var offsetToTarget = target.Location - Self.Location;

                var stepDirection = (-offsetToTarget).SnapToStep(Self.Level.Random);

                Self.Move(Self.Location + stepDirection, Self.Level);
            }

            public override void GoToState<TState>(Func<TState> factory)
            {
                Self.state = factory();
                Self.state.Tick();
            }

            private readonly Pawn target;
        }
    }
}