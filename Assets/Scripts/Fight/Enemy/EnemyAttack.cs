using UnityEngine;

namespace Fight.Enemy
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
            DecreaseCooldownTime();

            if (IsCoolingDown())
            {
                return;
            }

            if (IsPlayerInRange())
            {
                ReplenishCooldownTime();
                PlayAnimation();
                Attack();
            }
        }

        private bool IsCoolingDown()
        {
            return _cooldownTimeRemain > 0;
        }

        private bool IsPlayerInRange()
        {
            var hit = CastBox();
            return hit.collider != null;
        }

        private RaycastHit2D CastBox()
        {
            return Physics2D.BoxCast(
                boxCollider.bounds.center + transform.right * (distance * Mathf.Sign(enemySprite.localScale.x)),
                new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y), 0, Vector2.left, 0, enemyLayerMask);
        }

        private void ReplenishCooldownTime()
        {
            _cooldownTimeRemain = cooldownTime;
        }

        private void DecreaseCooldownTime()
        {
            _cooldownTimeRemain -= Time.deltaTime;
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