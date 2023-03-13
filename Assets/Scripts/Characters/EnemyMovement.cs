using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Patrolling Path")]
        [SerializeField] private Transform rightEdge;
        [SerializeField] private Transform leftEdge;

        [Header("Movement Parameters")] 
        [SerializeField] private float speed;
        [SerializeField] private float acceleration;
        [SerializeField] private float decceleration;
        [SerializeField] private float velocityPower;

        [Header("Visual")]
        [SerializeField] private Transform enemySprite;
        [SerializeField] private Animator animator;
        
        private Rigidbody2D _rigidbody;
        private Direction _currentDirection;
        
        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        private enum Direction
        {
            Left, Right
        }

        private void Awake()
        {
            GetReferences();
        }

        private void Start()
        {
            rightEdge.parent = null;
            leftEdge.parent = null;
        }

        private void Update()
        {
            HandleFlipping();
            SetAnimation();

            MoveInDirection(_currentDirection == Direction.Right ? 1 : -1);

            if (HasReachedLeftEdge())
                _currentDirection = Direction.Right;
            if (HasReachedRightEdge())
                _currentDirection = Direction.Left;
        }
        
        private void MoveInDirection(float input)
        {
            float targetSpeed = input * speed;
            float speedDif = targetSpeed - _rigidbody.velocity.x;
            float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01 ? acceleration : decceleration);
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelerationRate, velocityPower) * Mathf.Sign(speedDif);
            
            _rigidbody.AddForce(movement * Vector2.right);
        }

        private bool HasReachedRightEdge()
        {
            return _rigidbody.position.x > rightEdge.position.x && _currentDirection == Direction.Right;
        }

        private bool HasReachedLeftEdge()
        {
            return _rigidbody.position.x < leftEdge.position.x && _currentDirection == Direction.Left;
        }
        
        private void SetAnimation()
        {
            if (animator == null)
            {
                return;
            }
            animator?.SetFloat(SpeedHash, Mathf.Abs(_rigidbody.velocity.x));
        }

        private void HandleFlipping()
        {
            if (_rigidbody.velocity.x >= 0.01)
            {
                enemySprite.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (_rigidbody.velocity.x <= -0.01f)
            {
                enemySprite.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        
        private void GetReferences()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}