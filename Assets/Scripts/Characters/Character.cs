using GameLogic;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour, IDamageble
    {
        [SerializeField] protected Animator animator;
        [SerializeField] private float health;
        
        private float _health;

        protected virtual void Start()
        {
            _health = health;
        }

        public void TakeDamage(float damage)
        {
            _health = Mathf.Max(0, _health - damage);
            if (_health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            SetAnimation();
            Events.RaisePlayerDied();
            Destroy(gameObject);
        }

        private void SetAnimation()
        {
            animator.SetTrigger("Death");
        }
    }
}