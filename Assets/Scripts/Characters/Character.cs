using System;
using UnityEngine;

namespace Fight
{
    public class Character : MonoBehaviour, IDamageble
    {
        [SerializeField] protected Animator animator;
        
        private float _health;

        protected virtual void Start()
        {
            _health = 100f;
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
            animator.SetTrigger("Death");
        }
    }
}