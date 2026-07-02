using _Project.Gameplay;
using UnityEngine;

namespace _Project.Gameplay.States
{
    public class GameplayState : IState
    {
        private readonly GameContext context;

        public GameplayState(GameContext context)
        {
            this.context = context;
        }

        public void Enter()
        {
            HideResultScreens();
            SetExitOpened(false);
            SubscribeToDiamonds();
            context.RoundService.EnableRoundActors();
            TryOpenExit();
        }

        public void Exit()
        {
            UnsubscribeFromDiamonds();
        }

        private void SubscribeToDiamonds()
        {
            if (context.DiamondPickedCounter != null)
            {
                context.DiamondPickedCounter.OnDiamondPicked += TryOpenExit;
            }
        }

        private void UnsubscribeFromDiamonds()
        {
            if (context.DiamondPickedCounter != null)
            {
                context.DiamondPickedCounter.OnDiamondPicked -= TryOpenExit;
            }
        }

        private void TryOpenExit()
        {
            if (context.DiamondPickedCounter == null)
            {
                Debug.LogError("Diamond picked counter is not assigned.");
                return;
            }

            if (context.DiamondSpawnConfig == null)
            {
                Debug.LogError("Diamond spawn config is not assigned.");
                return;
            }

            if (context.DiamondPickedCounter.DiamondPickedCount >= context.DiamondSpawnConfig.Count)
            {
                SetExitOpened(true);
            }
        }

        private void SetExitOpened(bool isOpened)
        {
            if (context.ExitWall != null)
            {
                context.ExitWall.SetActive(!isOpened);
            }
        }

        private void HideResultScreens()
        {
            context.WinView.Hide();
            context.LoseView.Hide();
        }
    }
}
