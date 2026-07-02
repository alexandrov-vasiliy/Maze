using UnityEngine;

namespace _Project.Diamonds
{
    public class DiamondFactory : MonoBehaviour
    {
        [SerializeField] private Diamond diamondPrefab;
        [SerializeField] private DiamondPickedCounter diamondPickedCounter;

        public Diamond Create(Vector3 position, Quaternion rotation)
        {
            if (diamondPrefab == null)
            {
                Debug.LogError("Diamond prefab is not assigned.", this);
                return null;
            }

            if (diamondPickedCounter == null)
            {
                Debug.LogError("Diamond picked counter is not assigned.", this);
                return null;
            }

            Diamond diamond = Instantiate(diamondPrefab, position, rotation);
            diamond.Init(diamondPickedCounter);
            return diamond;
        }
    }
}
