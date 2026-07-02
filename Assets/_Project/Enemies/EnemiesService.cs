using System;
using UnityEngine;

namespace _Project.Enemies
{
    public class EnemiesService : MonoBehaviour
    {
        public event Action PlayerCaught;

        [SerializeField] private EnemyPatrolChase[] enemies;

        public void Init(Transform player)
        {
            if (enemies == null || player == null)
            {
                return;
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    enemies[i].PlayerCaught -= OnEnemyPlayerCaught;
                    enemies[i].PlayerCaught += OnEnemyPlayerCaught;
                    enemies[i].Init(player);
                }
            }
        }

        public void EnableEnemies()
        {
            if (enemies == null)
            {
                return;
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    enemies[i].EnableEnemy();
                }
            }
        }

        public void DisableEnemies()
        {
            if (enemies == null)
            {
                return;
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    enemies[i].DisableEnemy();
                }
            }
        }

        private void OnDestroy()
        {
            if (enemies == null)
            {
                return;
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    enemies[i].PlayerCaught -= OnEnemyPlayerCaught;
                }
            }
        }

        private void OnEnemyPlayerCaught()
        {
            PlayerCaught?.Invoke();
        }
    }
}
