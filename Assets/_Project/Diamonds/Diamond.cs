using _Project.Player;
using UnityEngine;

namespace _Project.Diamonds
{
    public class Diamond : MonoBehaviour
    {
        private DiamondPickedCounter diamondPickedCounter;

        public void Init(DiamondPickedCounter counter)
        {
            diamondPickedCounter = counter;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerRoot>(out _))
            {
                if (diamondPickedCounter == null)
                {
                    Debug.LogError("Diamond picked counter is not initialized.", this);
                    return;
                }

                diamondPickedCounter.PickedDiamond();
                Destroy(gameObject);
            }
        }
    }
}
