using DG.Tweening;
using UnityEngine;

namespace Fight.Enemy
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] private float shootingTime;
        [SerializeField] private float range;

        public void FireInDirection(Vector2 direction)
        {
            var tween = transform.DOMoveX(transform.position.x + direction.x * range, shootingTime);
            tween.OnComplete(DestroyFireball);
        }

        private void DestroyFireball()
        {
            Destroy(gameObject);
        }
    }
}