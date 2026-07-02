using System;
using _Project.Diamonds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Gameplay.Views
{
    public class WinView : MonoBehaviour
    {
        public event Action RestartClicked;

        [SerializeField] private Button restartButton;
        [SerializeField] private TMP_Text diamondsText;

        private DiamondPickedCounter diamondPickedCounter;

        public void Init(DiamondPickedCounter counter)
        {
            diamondPickedCounter = counter;
            restartButton.onClick.AddListener(Restart);
        }

        public void Show()
        {
            diamondsText.text = $"Собрано алмазов: {diamondPickedCounter.DiamondPickedCount}";
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
