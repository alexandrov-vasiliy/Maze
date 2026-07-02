using System;
using _Project.Diamonds;
using _Project.Enemies;
using _Project.Player;
using UnityEngine;

namespace _Project.Gameplay
{
    public class RoundService : MonoBehaviour
    {
        public event Action PlayerCaught;

        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DiamondsSpawner diamondsSpawner;
        [SerializeField] private DiamondPickedCounter diamondPickedCounter;
        [SerializeField] private EnemiesService enemiesService;

        private GameObject playerInstance;
        private PlayerMovement playerMovement;

        public void PrepareRound()
        {
            ClearPlayer();
            diamondsSpawner?.Clear();
            SpawnPlayer();
            DisableRoundActors();
            InitEnemies();
            ResetDiamonds();
            SpawnDiamonds();
        }

        public void EnableRoundActors()
        {
            playerMovement?.EnableMovement();
            enemiesService?.EnableEnemies();
        }

        public void DisableRoundActors()
        {
            playerMovement?.DisableMovement();
            enemiesService?.DisableEnemies();
        }

        private void OnDestroy()
        {
            if (enemiesService != null)
            {
                enemiesService.PlayerCaught -= OnPlayerCaught;
            }
        }

        private void SpawnPlayer()
        {
            if (playerPrefab == null || spawnPoint == null)
            {
                Debug.LogError("Player prefab or spawn point is not assigned.", this);
                return;
            }

            playerInstance = Instantiate(
                playerPrefab,
                spawnPoint.position,
                spawnPoint.rotation);

            playerMovement = playerInstance.GetComponent<PlayerMovement>();

            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement component is not found on player prefab.", playerInstance);
            }
        }

        private void ClearPlayer()
        {
            if (playerInstance != null)
            {
                Destroy(playerInstance);
            }

            playerInstance = null;
            playerMovement = null;
        }

        private void InitEnemies()
        {
            if (enemiesService == null)
            {
                Debug.LogError("Enemies service is not assigned.", this);
                return;
            }

            enemiesService.PlayerCaught -= OnPlayerCaught;
            enemiesService.PlayerCaught += OnPlayerCaught;
            enemiesService.Init(playerInstance != null ? playerInstance.transform : null);
            enemiesService.DisableEnemies();
        }

        private void ResetDiamonds()
        {
            if (diamondPickedCounter == null)
            {
                Debug.LogError("Diamond picked counter is not assigned.", this);
                return;
            }

            diamondPickedCounter.ResetCounter();
        }

        private void SpawnDiamonds()
        {
            if (diamondsSpawner == null)
            {
                Debug.LogError("Diamonds spawner is not assigned.", this);
                return;
            }

            diamondsSpawner.Spawn();
        }

        private void OnPlayerCaught()
        {
            PlayerCaught?.Invoke();
        }
    }
}
