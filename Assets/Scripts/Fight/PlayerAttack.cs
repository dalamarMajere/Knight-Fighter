using System;
using UnityEngine;

namespace Fight
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private Animator attackAnimation;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] private LayerMask enemyLayerMask;
        
        private readonly int AttackHash = Animator.StringToHash("Attack");

        private void Update()
        {
            if (HasInput())
            {
                Attack();
            }
        }

        private void Attack()
        {
            PlayAnimation();

            var hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayerMask);
            
            foreach (var hitEnemy in hitEnemies)
            {
                if (hitEnemy.TryGetComponent<IDamageble>(out var damageble))
                {
                    damageble.TakeDamage(damage);
                }
            }
        }
        
        private bool HasInput()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        private void PlayAnimation()
        {
            attackAnimation.SetTrigger(AttackHash);
        }
    }
}
