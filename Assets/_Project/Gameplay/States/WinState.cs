using _Project.Gameplay;
using UnityEngine;

namespace _Project.Gameplay.States
{
    public class WinState : IState
    {
        private readonly GameContext context;

        public WinState(GameContext context)
        {
            this.context = context;
        }

        public void Enter()
        {
            context.RoundService.DisableRoundActors();
            context.WinView.Show();
            context.LoseView.Hide();
        }

        public void Exit()
        {
        }
    }
}
