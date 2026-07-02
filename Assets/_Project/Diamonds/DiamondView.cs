using TMPro;
using UnityEngine;

namespace _Project.Diamonds
{
    public class DiamondView : MonoBehaviour
    {
        [SerializeField] private DiamondPickedCounter diamondPickedCounter;
        [SerializeField] private DiamondSpawnConfig diamondSpawnConfig;
        [SerializeField] private TMP_Text diamondText;

        private void OnEnable()
        {
            if (diamondPickedCounter == null)
            {
                Debug.LogError("Diamond picked counter is not assigned.", this);
                return;
            }

            diamondPickedCounter.OnDiamondPicked += UpdateText;
            UpdateText();
        }

        private void OnDisable()
        {
            if (diamondPickedCounter != null)
            {
                diamondPickedCounter.OnDiamondPicked -= UpdateText;
            }
        }

        private void UpdateText()
        {
            if (diamondText == null)
            {
                Debug.LogError("Diamond text is not assigned.", this);
                return;
            }

            if (diamondSpawnConfig == null)
            {
                Debug.LogError("Diamond spawn config is not assigned.", this);
                return;
            }

            diamondText.text = $"{diamondPickedCounter.DiamondPickedCount}/{diamondSpawnConfig.Count}";
        }
    }
}
