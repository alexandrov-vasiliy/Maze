using System;
using UnityEngine;

namespace _Project.Diamonds
{
    public class DiamondPickedCounter : MonoBehaviour
    {
        public event Action OnDiamondPicked;
        public int DiamondPickedCount { get; private set; } = 0;


        public void PickedDiamond()
        {
            DiamondPickedCount++;
            OnDiamondPicked?.Invoke();
        }

        public void ResetCounter()
        {
            DiamondPickedCount = 0;
            OnDiamondPicked?.Invoke();
        }
    }
}
