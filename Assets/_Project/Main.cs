using UnityEngine;
using _Project.Gameplay;
using _Project.Gameplay.States;

namespace _Project
{
    [DefaultExecutionOrder(-1)]
    public class Main : MonoBehaviour
    {
        [SerializeField] private GameContext context = new();

        private GameStateMachine stateMachine;

        private void Awake()
        {
            stateMachine = new GameStateMachine();
            stateMachine.RegisterState(new LoadingState(stateMachine, context));
            stateMachine.RegisterState(new GameplayState(context));
            stateMachine.RegisterState(new WinState(context));
            stateMachine.RegisterState(new LoseState(context));

            if (context.WinTrigger != null)
            {
                context.WinTrigger.PlayerReachedExit += OnPlayerReachedExit;
            }

            if (context.RoundService != null)
            {
                context.RoundService.PlayerCaught += OnPlayerCaught;
            }

            context.WinView.RestartClicked += Restart;
            context.LoseView.RestartClicked += Restart;

            context.WinView.Init(context.DiamondPickedCounter);
            context.LoseView.Init();
        }

        private void Start()
        {
            stateMachine.ChangeState<LoadingState>();
        }

        private void OnDestroy()
        {
            if (context.WinTrigger != null)
            {
                context.WinTrigger.PlayerReachedExit -= OnPlayerReachedExit;
            }

            if (context.RoundService != null)
            {
                context.RoundService.PlayerCaught -= OnPlayerCaught;
            }

            if (context.WinView != null)
            {
                context.WinView.RestartClicked -= Restart;
            }

            if (context.LoseView != null)
            {
                context.LoseView.RestartClicked -= Restart;
            }
        }

        private void OnPlayerReachedExit()
        {
            stateMachine.ChangeState<WinState>();
        }

        private void OnPlayerCaught()
        {
            stateMachine.ChangeState<LoseState>();
        }

        private void Restart()
        {
            stateMachine.ChangeState<LoadingState>();
        }
    }
}
