using System;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Animator animator;

        private readonly Vector2 _forwardDirection = Vector2.right;
        
        private Rigidbody2D _rigidbody;
        private float _horizontalInput;
        private static readonly int SpeedAnimationProperty = Animator.StringToHash("Speed");

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            GetInput();

            SetAnimationSpeed();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
            _rigidbody.velocity = _forwardDirection * (_horizontalInput * speed);
        }

        private void GetInput()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
        }
        
        private void SetAnimationSpeed()
        {
            animator.SetFloat(SpeedAnimationProperty, HasInput() ? speed : 0);
        }

        private bool HasInput() => _horizontalInput != 0;
    }
}