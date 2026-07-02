using _Project.Gameplay;

namespace _Project.Gameplay.States
{
    public class LoadingState : IState
    {
        private readonly GameStateMachine stateMachine;
        private readonly GameContext context;

        public LoadingState(GameStateMachine stateMachine, GameContext context)
        {
            this.stateMachine = stateMachine;
            this.context = context;
        }

        public void Enter()
        {
            HideResultScreens();
            context.RoundService.PrepareRound();

            stateMachine.ChangeState<GameplayState>();
        }

        public void Exit()
        {
        }

        private void HideResultScreens()
        {
            context.WinView.Hide();
            context.LoseView.Hide();
        }
    }
}
