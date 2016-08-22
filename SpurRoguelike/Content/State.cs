using System;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.Content
{
    internal abstract class State<T> where T : Pawn
    {
        protected State(T self)
        {
            Self = self;
        }

        public abstract void Tick();

        public abstract void GoToState<TState>(Func<TState> factory) where TState : State<T>; 

        protected T Self;
    }
}