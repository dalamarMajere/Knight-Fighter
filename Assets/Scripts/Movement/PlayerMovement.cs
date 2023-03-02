using System;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Characteristics")]
        [SerializeField] private float speed;
        [SerializeField] private float acceleration = 7;
        [SerializeField] private float decceleration = 7;
        [SerializeField] private float velocityPower = 0.9f;
        [SerializeField] private float frictionAmount = 0.2f;
        
        [Header("Jump Characteristics")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float coyoteTime;
        [SerializeField] private float gravityScale;
        [SerializeField] private float fallGravityScale;
        
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
        private float _lastGroundedTime;
        private bool _isJumping;
        
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
            AdjustGravity();

            SetAnimationSpeed();
            SetPlayerDirection();

            DecreaseJumpingTime();
        }

        private void AdjustGravity()
        {
            if (_rigidbody.velocity.y < 0)
            {
                _rigidbody.gravityScale = gravityScale * fallGravityScale;
            }
            else
            {
                _rigidbody.gravityScale = gravityScale;
            }
        }

        private void DecreaseJumpingTime()
        {
            _lastGroundedTime -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            MovePlayer();
            AddFriction();
        }

        private void TryJumping()
        {
            if (IsGrounded())
            {
                _lastGroundedTime = coyoteTime;
            }   
            
            if (_jumpingInput && _lastGroundedTime > 0)
            {
                Jump();
            }
        }

        private void Jump()
        {
            float force = jumpForce;
            if (_rigidbody.velocity.y < 0)
                force -= _rigidbody.velocity.y;

            _rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
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
            float targetSpeed = _horizontalInput * speed;
            float speedDif = targetSpeed - _rigidbody.velocity.x;
            float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01 ? acceleration : decceleration);
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelerationRate, velocityPower) * Mathf.Sign(speedDif);
            
            _rigidbody.AddForce(movement * _forwardDirection);
        }
        
        private void AddFriction()
        {
            if (_isGrounded & Mathf.Abs(_horizontalInput) < 0.01f)
            {
                float amount = Mathf.Min(Mathf.Abs(_rigidbody.velocity.x), Mathf.Abs(frictionAmount));
                amount *= Mathf.Sign(_rigidbody.velocity.x);
                _rigidbody.AddForce(_forwardDirection * -amount, ForceMode2D.Impulse);
            }
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