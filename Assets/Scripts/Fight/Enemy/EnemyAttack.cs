using UnityEngine;

namespace Fight
{
    public abstract class EnemyAttack : CharacterAttack
    {
        [Header("Player Finding")]
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private float distance;
        [SerializeField] private float range;
        [SerializeField] protected Transform enemySprite;
    
        [Header("Cooldown")]
        [SerializeField] private float cooldownTime;

        private float _cooldownTimeRemain;

        private void Update()
        {
            _cooldownTimeRemain -= Time.deltaTime;

            if (_cooldownTimeRemain > 0)
            {
                return;
            }

            if (IsPlayerInRange())
            {
                _cooldownTimeRemain = cooldownTime;
                PlayAnimation();
                Attack();
            }
        }

        private bool IsPlayerInRange()
        {
            var hit = Physics2D.BoxCast(
                boxCollider.bounds.center + transform.right * (distance * Mathf.Sign(enemySprite.localScale.x)),
                new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y), 0, Vector2.left, 0, enemyLayerMask);
            return hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                boxCollider.bounds.center + transform.right * (distance * Mathf.Sign(enemySprite.localScale.x)),
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, 0));
        }
    }
}