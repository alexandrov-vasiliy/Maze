using UnityEngine;

namespace _Project.Common
{
    public class ObjectRotator : MonoBehaviour
    {
        [SerializeField] private Vector3 axis = Vector3.up;
        [SerializeField] private float speed = 90f;
        [SerializeField] private Space space = Space.Self;

        private void Update()
        {
            transform.Rotate(axis, speed * Time.deltaTime, space);
        }
    }
}
