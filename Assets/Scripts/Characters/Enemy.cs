using Pathfinding;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Seeker))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : Character
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 200;
        [SerializeField] private float nextWaypointDistance = 3f;
        
        [Header("Ground Checking")] 
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float raycastDistance;
        [SerializeField] private LayerMask platformLayerMask;
        
        [Header("Visual")] 
        [SerializeField] private Transform enemySprite;

        private Path _path;
        int _currentWaypoint;
        bool _hasReachedEndOfPath;

        private Seeker _seeker;
        private Rigidbody2D _rigidbody2D;
        
        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        private void Awake()
        {
            GetReferences();
        }

        protected override void Start()
        {
            base.Start();
            
            InvokeRepeating("UpdatePath", 0f, 0.5f );

            UpdatePath();
        }

        private void Update()
        {
            HandleFlipping();
            SetAnimation();

            if (IsPathInitialized())
            {
                return;
            }
            
            if (HasReachedEndOfPath())
            {
                _hasReachedEndOfPath = true;
                return;
            }

            _hasReachedEndOfPath = false;
        }
        
        private void FixedUpdate()
        {
            if (IsPathInitialized() || _hasReachedEndOfPath)
            {
                return;
            }
            
            TryIncreasingWaypointIndex();
            
            if (!CanMove())
            {
                Stop();
                return;
            }
            
            MoveEnemy();
        }

        private void Stop()
        {
            _rigidbody2D.velocity = Vector3.zero;
        }

        private void MoveEnemy()
        {
            Vector2 direction = GetDirection();
            Vector2 force = direction * (speed * Time.deltaTime);

            _rigidbody2D.AddForce(force);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        }

        private bool CanMove()
        {
            var hit = Physics2D.Raycast((Vector2)groundCheckPoint.position + GetDirection(),  Vector2.down, raycastDistance, platformLayerMask);
            Debug.DrawLine((Vector2)groundCheckPoint.position + GetDirection(), (Vector2)groundCheckPoint.position + GetDirection() + raycastDistance * Vector2.down);
            return hit.collider != null;
        }
        
        private void TryIncreasingWaypointIndex()
        {
            float distance = GetDistanceBetweenWaypoints();

            if (distance < nextWaypointDistance)
            {
                _currentWaypoint++;
            }
        }

        private void UpdatePath()
        {
            if (_seeker.IsDone())
            {
                _seeker.StartPath(_rigidbody2D.position, target.position, OnPathComplete);
            }
        }
        
        private void OnPathComplete(Path p)
        {
            if (p.error)
            {
                return;
            }

            _path = p;
            _currentWaypoint = 0;
        }
        
        private void HandleFlipping()
        {
            if (_rigidbody2D.velocity.x >= 0.01)
            {
                enemySprite.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (_rigidbody2D.velocity.x <= -0.01f)
            {
                enemySprite.localScale = new Vector3(-1f, 1f, 1f);
            }
        }

        private float GetDistanceBetweenWaypoints()
        {
            return Vector2.Distance(_rigidbody2D.position, _path.vectorPath[_currentWaypoint]);
        }

        private Vector2 GetDirection()
        {
            return ((Vector2)_path.vectorPath[Mathf.Min(_currentWaypoint, _path.vectorPath.Count)] - _rigidbody2D.position).normalized;
        }

        private bool HasReachedEndOfPath()
        {
            return _currentWaypoint >= _path.vectorPath.Count;
        }

        private bool IsPathInitialized()
        {
            return _path == null;
        }
        
        private void SetAnimation()
        {
            animator.SetFloat(SpeedHash, Mathf.Abs(_rigidbody2D.velocity.x));
        }

        private void GetReferences()
        {
            _seeker = GetComponent<Seeker>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}