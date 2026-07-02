using _Project.Gameplay;
using UnityEngine;

namespace _Project.Gameplay.States
{
    public class LoseState : IState
    {
        private readonly GameContext context;

        public LoseState(GameContext context)
        {
            this.context = context;
        }

        public void Enter()
        {
            context.RoundService.DisableRoundActors();
            context.WinView.Hide();
            context.LoseView.Show();
        }

        public void Exit()
        {
        }
    }
}
