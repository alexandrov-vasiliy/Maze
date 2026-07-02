using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference sprintAction;

        [Header("Camera")]
        [SerializeField] private Transform cameraTransform;

        [Header("Movement")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float sprintSpeed = 8f;

        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();

            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void OnEnable()
        {
            moveAction.action.Enable();
            sprintAction.action.Enable();
        }

        private void OnDisable()
        {
            moveAction.action.Disable();
            sprintAction.action.Disable();
        }

        public void EnableMovement()
        {
            enabled = true;
        }

        public void DisableMovement()
        {
            enabled = false;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 direction = right * moveInput.x + forward * moveInput.y;

            if (direction.sqrMagnitude > 1f)
            {
                direction.Normalize();
            }

            float speed = sprintAction.action.IsPressed() ? sprintSpeed : walkSpeed;
            Vector3 velocity = direction * speed;
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
