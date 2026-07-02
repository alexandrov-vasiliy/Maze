using UnityEngine;

namespace _Project.Diamonds
{
    [CreateAssetMenu(menuName = "Configs/Diamond Spawn Config")]
    public class DiamondSpawnConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public int Count { get; private set; } = 5;
    }
}
