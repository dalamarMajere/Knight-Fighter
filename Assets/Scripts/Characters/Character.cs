using System;
using UnityEngine;

namespace Fight
{
    public class Character : MonoBehaviour, IDamageble
    {
        private float _health;

        protected virtual void Start()
        {
            _health = 100f;
        }

        public void TakeDamage(float damage)
        {
            _health = Mathf.Max(0, _health - damage);
        }
    }
}