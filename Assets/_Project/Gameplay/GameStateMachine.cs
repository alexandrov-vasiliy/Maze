using System;
using System.Collections.Generic;

namespace _Project.Gameplay
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IState> states = new();

        private IState currentState;

        public void RegisterState<TState>(TState state) where TState : IState
        {
            states[typeof(TState)] = state;
        }

        public void ChangeState<TState>() where TState : IState
        {
            if (!states.TryGetValue(typeof(TState), out IState nextState))
            {
                throw new InvalidOperationException($"State {typeof(TState).Name} is not registered.");
            }

            currentState?.Exit();
            currentState = nextState;
            currentState.Enter();
        }
    }
}
