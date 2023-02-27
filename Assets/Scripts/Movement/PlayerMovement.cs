using System;
using DG.Tweening;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Characteristics")]
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [Header("Other")] [SerializeField]
        private LayerMask platformLayerMask;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject playerSprite;
        
        private Rigidbody2D _rigidbody;
        private float _horizontalInput;
        private bool _jumpingInput;
        private bool _isGrounded;
        private BoxCollider2D _boxCollider2D;
        private PlayerSpriteDirection _playerSpriteDirection;
        
        private static readonly int SpeedAnimationProperty = Animator.StringToHash("Speed");
        private readonly Vector2 _forwardDirection = Vector2.right;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            _playerSpriteDirection = new(playerSprite);
        }

        private void Update()
        {
            GetInput();

            TryJumping();

            SetAnimationSpeed();
            SetPlayerDirection();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void TryJumping()
        {
            if (_jumpingInput && IsGrounded())
            {
                _rigidbody.velocity += Vector2.up * jumpForce;
            }
        }

        private bool IsGrounded()
        {
            var hit = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, platformLayerMask);
            return hit.collider != null;
        }

        private void SetPlayerDirection()
        {
            _playerSpriteDirection.SetSpriteDirectionByInput(_horizontalInput);
        }

        private void MovePlayer()
        {
            _rigidbody.velocity = _forwardDirection * (_horizontalInput * speed) + _rigidbody.velocity.y * Vector2.up;
        }

        private void GetInput()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _jumpingInput = Input.GetKeyDown(KeyCode.W);
        }
        
        private void SetAnimationSpeed()
        {
            animator.SetFloat(SpeedAnimationProperty, HasInput() ? speed : 0);
        }

        private bool HasInput() => _horizontalInput != 0;
    }
}