using System;
using Characters;
using DG.Tweening;
using UnityEngine;

namespace Fight.Enemy
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] private float shootingTime;
        [SerializeField] private float range;
        
        public float Damage { set; get; }

        private const string PlayerTag = "Player";

        public void FireInDirection(Vector2 direction)
        {
            var tween = transform.DOMoveX(transform.position.x + direction.x * range, shootingTime);
            tween.OnComplete(DestroyFireball);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (IsPlayer(col))
            {
                col.gameObject.TryGetComponent<IDamageble>(out var damageble);
                damageble.TakeDamage(Damage);
                DestroyFireball(); 
            }
        }
        
        private static bool IsPlayer(Collider2D collision)
        {
            return collision.gameObject.CompareTag(PlayerTag);
        }

        private void DestroyFireball()
        {
            Destroy(gameObject);
        }
    }
}