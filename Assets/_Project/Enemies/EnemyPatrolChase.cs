using System;
using _Project.Player;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Project.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyPatrolChase : MonoBehaviour
    {
        public event Action PlayerCaught;

        private enum EnemyState
        {
            Patrol,
            Chase
        }

        [Header("Patrol")]
        [SerializeField] private Vector3 patrolZoneSize = new(10f, 2f, 10f);
        [SerializeField, Min(0.1f)] private float patrolPointSampleDistance = 2f;
        [SerializeField, Min(1)] private int maxPatrolPointAttempts = 20;
        [SerializeField, Min(0f)] private float minDistanceToNextPatrolPoint = 2f;
        [SerializeField, Min(0f)] private float waitAtPointTime = 1f;

        [Header("Detection")]
        [SerializeField, Min(0f)] private float detectionRadius = 6f;
        [SerializeField, Min(0f)] private float loseTargetRadius = 9f;

        [Header("Movement")]
        [SerializeField, Min(0f)] private float patrolSpeed = 2.5f;
        [SerializeField, Min(0f)] private float chaseSpeed = 4.5f;

        [Header("Gizmos")]
        [SerializeField] private Color patrolZoneColor = new(0.2f, 0.7f, 1f, 0.25f);
        [SerializeField] private Color detectionRadiusColor = new(1f, 0.85f, 0.1f, 0.8f);
        [SerializeField] private Color loseTargetRadiusColor = new(1f, 0.45f, 0.1f, 0.8f);

        private NavMeshAgent agent;
        private Transform player;
        private Vector3 patrolZoneCenter;
        private Vector3 startPosition;
        private Quaternion startRotation;
        private Vector3 currentPatrolPoint;
        private EnemyState state;
        private float waitTimer;
        private bool isInitialized;
        private bool isActive;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            startPosition = transform.position;
            startRotation = transform.rotation;
            patrolZoneCenter = transform.position;
        }

        public void Init(Transform playerTarget)
        {
            player = playerTarget;
            isInitialized = player != null;
            ResetEnemy();
        }

        public void EnableEnemy()
        {
            isActive = isInitialized;
            agent.enabled = isActive;

            if (isActive)
            {
                SetPatrolState();
            }
        }

        public void DisableEnemy()
        {
            isActive = false;

            if (agent.enabled)
            {
                agent.ResetPath();
            }
        }

        public void ResetEnemy()
        {
            state = EnemyState.Patrol;
            waitTimer = 0f;

            if (agent != null)
            {
                agent.speed = patrolSpeed;

                if (agent.enabled)
                {
                    agent.ResetPath();
                    agent.Warp(startPosition);
                }
                else
                {
                    transform.position = startPosition;
                }
            }

            transform.rotation = startRotation;
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            switch (state)
            {
                case EnemyState.Patrol:
                    UpdatePatrol();
                    break;
                case EnemyState.Chase:
                    UpdateChase();
                    break;
            }
        }

        private void UpdatePatrol()
        {
            if (IsPlayerInsideRadius(detectionRadius))
            {
                SetChaseState();
                return;
            }

            if (!agent.hasPath)
            {
                SetNextPatrolPoint();
                return;
            }

            if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                return;
            }

            waitTimer += Time.deltaTime;

            if (waitTimer >= waitAtPointTime)
            {
                SetNextPatrolPoint();
            }
        }

        private void UpdateChase()
        {
            if (!IsPlayerInsideRadius(loseTargetRadius))
            {
                SetPatrolState();
                return;
            }

            agent.SetDestination(player.position);
        }

        private void SetPatrolState()
        {
            state = EnemyState.Patrol;
            agent.speed = patrolSpeed;
            SetNextPatrolPoint();
        }

        private void SetChaseState()
        {
            state = EnemyState.Chase;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }

        private void SetNextPatrolPoint()
        {
            waitTimer = 0f;

            if (TryGetRandomPointInPatrolZone(out currentPatrolPoint))
            {
                agent.SetDestination(currentPatrolPoint);
            }
        }

        private bool TryGetRandomPointInPatrolZone(out Vector3 point)
        {
            Vector3 halfSize = patrolZoneSize * 0.5f;
            float minSqrDistance = minDistanceToNextPatrolPoint * minDistanceToNextPatrolPoint;

            for (int i = 0; i < maxPatrolPointAttempts; i++)
            {
                Vector3 randomPoint = new(
                    Random.Range(patrolZoneCenter.x - halfSize.x, patrolZoneCenter.x + halfSize.x),
                    patrolZoneCenter.y,
                    Random.Range(patrolZoneCenter.z - halfSize.z, patrolZoneCenter.z + halfSize.z));

                if (!NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolPointSampleDistance, NavMesh.AllAreas))
                {
                    continue;
                }

                if ((hit.position - transform.position).sqrMagnitude < minSqrDistance)
                {
                    continue;
                }

                point = hit.position;
                return true;
            }

            point = transform.position;
            return false;
        }

        private bool IsPlayerInsideRadius(float radius)
        {
            float sqrRadius = radius * radius;
            return (player.position - transform.position).sqrMagnitude <= sqrRadius;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActive || !other.TryGetComponent<PlayerRoot>(out _))
            {
                return;
            }

            PlayerCaught?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 center = Application.isPlaying ? patrolZoneCenter : transform.position;

            Gizmos.color = patrolZoneColor;
            Gizmos.DrawCube(center, patrolZoneSize);
            Gizmos.DrawWireCube(center, patrolZoneSize);

            Gizmos.color = detectionRadiusColor;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Gizmos.color = loseTargetRadiusColor;
            Gizmos.DrawWireSphere(transform.position, loseTargetRadius);

            if (Application.isPlaying && currentPatrolPoint != Vector3.zero)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(currentPatrolPoint, 0.25f);
                Gizmos.DrawLine(transform.position, currentPatrolPoint);
            }
        }
    }
}
