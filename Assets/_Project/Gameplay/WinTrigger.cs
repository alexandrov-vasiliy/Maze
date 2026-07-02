using System;
using _Project.Player;
using UnityEngine;

namespace _Project.Gameplay
{
    public class WinTrigger : MonoBehaviour
    {
        public event Action PlayerReachedExit;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerRoot>(out _))
            {
                return;
            }

            PlayerReachedExit?.Invoke();
        }
    }
}
