using Characters;
using UnityEngine;

namespace Fight.Enemy
{
    public class RangedEnemyAttack : EnemyAttack
    {
        [Header("Fireball")]
        [SerializeField] private Fireball fireballPrefab;

        protected override void Attack()
        {
            var fireball = Instantiate(fireballPrefab, this.transform);
            fireball.transform.parent = null;
            
            fireball.FireInDirection(Vector2.right * enemySprite.localScale.x);
            fireball.Damage = damage;
        }
    }
}