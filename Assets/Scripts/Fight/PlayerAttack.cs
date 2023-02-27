using System;
using UnityEngine;

namespace Fight
{
    public class Enemy : MonoBehaviour
    {
        
    }
    
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Animator attackAnimation;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] private LayerMask enemyLayerMask;
        
        private readonly int AttackHash = Animator.StringToHash("Attack");

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
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
                // damage them    
            }
        }

        private void PlayAnimation()
        {
            attackAnimation.SetTrigger(AttackHash);
        }
    }
}
