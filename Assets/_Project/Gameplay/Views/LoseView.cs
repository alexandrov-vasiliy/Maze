using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Gameplay.Views
{
    public class LoseView : MonoBehaviour
    {
        public event Action RestartClicked;

        [SerializeField] private Button restartButton;

        public void Init()
        {
            restartButton.onClick.AddListener(Restart);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Restart()
        {
            RestartClicked?.Invoke();
        }
    }
}
