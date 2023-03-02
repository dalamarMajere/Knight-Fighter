using Characters;
using UnityEngine;

namespace Fight
{
    public abstract class CharacterAttack : MonoBehaviour
    {
        [Header("Attack Characteristics")]
        [SerializeField] private float damage;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] protected LayerMask enemyLayerMask;
        
        [Header("Visual")]
        [SerializeField] private Animator animator;
        
        private readonly int AttackHash = Animator.StringToHash("Attack");

        protected virtual void Attack()
        {
            var enemiesCollider = GetHitColliders();
            
            HitEnemies(enemiesCollider);
        }
        
        protected void PlayAnimation()
        {
            animator.SetTrigger(AttackHash);
        }
        
        private void HitEnemies(Collider2D[] hitEnemies)
        {
            foreach (var hitEnemy in hitEnemies)
            {
                if (hitEnemy.TryGetComponent<IDamageble>(out var damageble))
                {
                    damageble.TakeDamage(damage);
                }
            }
        }

        private Collider2D[] GetHitColliders()
        {
            return Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayerMask);
        }
    }
}