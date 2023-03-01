using UnityEngine;
using Pathfinding;

namespace Fight
{
    public class Enemy : Character
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 200;
        [SerializeField] private float nextWaypointDistance = 3f;
    }
}