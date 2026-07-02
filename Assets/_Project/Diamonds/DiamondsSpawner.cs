using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Diamonds
{
    public class DiamondsSpawner : MonoBehaviour
    {
        [SerializeField] private DiamondSpawnConfig diamondSpawnConfig;
        [SerializeField] private DiamondFactory diamondFactory;
        [SerializeField] private NavMeshSurface navMeshSurface;
        [SerializeField] private float sampleDistance = 2f;
        [SerializeField] private int maxAttemptsPerDiamond = 20;
        [SerializeField, Min(0f)] private float minDistanceBetweenDiamonds = 3f;
        [SerializeField, Min(0f)] private float minDistanceFromNavMeshEdge = 2f;
        [SerializeField] private float yOffset;

        private readonly List<Diamond> spawnedDiamonds = new();

        public void Spawn()
        {
            if (diamondSpawnConfig == null)
            {
                Debug.LogError("Diamond spawn config is not assigned.", this);
                return;
            }

            if (diamondFactory == null)
            {
                Debug.LogError("Diamond factory is not assigned.", this);
                return;
            }

            if (navMeshSurface == null)
            {
                Debug.LogError("NavMeshSurface is not assigned.", this);
                return;
            }

            Bounds spawnBounds = GetNavMeshBounds();
            List<Vector3> spawnPoints = new();

            for (int i = 0; i < diamondSpawnConfig.Count; i++)
            {
                if (TryGetSpawnPoint(spawnBounds, spawnPoints, out Vector3 position))
                {
                    spawnedDiamonds.Add(diamondFactory.Create(position, Quaternion.identity));
                    spawnPoints.Add(position);
                }
                else
                {
                    Debug.LogError("Can not find point on NavMesh for diamond spawn.", this);
                    return;
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < spawnedDiamonds.Count; i++)
            {
                if (spawnedDiamonds[i] != null)
                {
                    Destroy(spawnedDiamonds[i].gameObject);
                }
            }

            spawnedDiamonds.Clear();
        }

        private bool TryGetSpawnPoint(Bounds spawnBounds, IReadOnlyList<Vector3> usedPositions, out Vector3 position)
        {
            for (int i = 0; i < maxAttemptsPerDiamond; i++)
            {
                if (TryGetRandomPointOnNavMesh(spawnBounds, out Vector3 randomPoint))
                {
                    position = randomPoint + Vector3.up * yOffset;

                    if (IsFarEnoughFromNavMeshEdge(randomPoint) &&
                        IsFarEnoughFromUsedPositions(position, usedPositions))
                    {
                        return true;
                    }
                }
            }

            position = Vector3.zero;
            return false;
        }

        private bool IsFarEnoughFromUsedPositions(Vector3 position, IReadOnlyList<Vector3> usedPositions)
        {
            float minSqrDistance = minDistanceBetweenDiamonds * minDistanceBetweenDiamonds;

            for (int i = 0; i < usedPositions.Count; i++)
            {
                if ((position - usedPositions[i]).sqrMagnitude < minSqrDistance)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsFarEnoughFromNavMeshEdge(Vector3 position)
        {
            return minDistanceFromNavMeshEdge <= 0f ||
                   NavMesh.FindClosestEdge(position, out NavMeshHit hit, NavMesh.AllAreas) &&
                   hit.distance >= minDistanceFromNavMeshEdge;
        }

        private bool TryGetRandomPointOnNavMesh(Bounds spawnBounds, out Vector3 position)
        {
            Vector3 point = new(
                Random.Range(spawnBounds.min.x, spawnBounds.max.x),
                spawnBounds.center.y,
                Random.Range(spawnBounds.min.z, spawnBounds.max.z));

            if (NavMesh.SamplePosition(point, out NavMeshHit hit, sampleDistance, NavMesh.AllAreas))
            {
                position = hit.position;
                return true;
            }

            position = Vector3.zero;
            return false;
        }

        private Bounds GetNavMeshBounds()
        {
            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

            if (triangulation.vertices.Length == 0)
            {
                return new Bounds(navMeshSurface.transform.position, navMeshSurface.size);
            }

            Bounds bounds = new(triangulation.vertices[0], Vector3.zero);

            for (int i = 1; i < triangulation.vertices.Length; i++)
            {
                bounds.Encapsulate(triangulation.vertices[i]);
            }

            return bounds;
        }
    }
}
