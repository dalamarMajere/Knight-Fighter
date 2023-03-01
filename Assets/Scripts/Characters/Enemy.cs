using System;
using UnityEngine;
using Pathfinding;

namespace Fight
{
    [RequireComponent(typeof(Seeker))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : Character
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 200;
        [SerializeField] private float nextWaypointDistance = 3f;
        [SerializeField] private Transform enemySprite;
        
        private Path _path;
        int _currentWaypoint;
        bool _hasReachedEndOfPath;

        private Seeker _seeker;
        private Rigidbody2D _rigidbody2D;

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

            MoveEnemy();
            TryIncreasingWaypointIndex();
            HandleFlipping();
        }
        
        private void TryIncreasingWaypointIndex()
        {
            float distance = GetDistanceBetweenWaypoints();

            if (distance < nextWaypointDistance)
            {
                _currentWaypoint++;
            }
        }

        private void MoveEnemy()
        {
            Vector2 direction = GetDirection();
            Vector2 force = direction * (speed * Time.deltaTime);

            _rigidbody2D.AddForce(force);
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
                enemySprite.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (_rigidbody2D.velocity.x <= -0.01f)
            {
                enemySprite.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        private float GetDistanceBetweenWaypoints()
        {
            return Vector2.Distance(_rigidbody2D.position, _path.vectorPath[_currentWaypoint]);
        }

        private Vector2 GetDirection()
        {
            return ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidbody2D.position).normalized;
        }

        private bool HasReachedEndOfPath()
        {
            return _currentWaypoint >= _path.vectorPath.Count;
        }

        private bool IsPathInitialized()
        {
            return _path == null;
        }

        private void GetReferences()
        {
            _seeker = GetComponent<Seeker>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}