using UnityEngine;

namespace Fight
{
    public class CharacterAttack : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] private LayerMask enemyLayerMask;
        
        private readonly int AttackHash = Animator.StringToHash("Attack");

        protected void Attack()
        {
            PlayAnimation();

            var enemiesCollider = GetHitColliders();
            
            HitEnemies(enemiesCollider);
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

        private void PlayAnimation()
        {
            animator.SetTrigger(AttackHash);
        }
        
    }
}