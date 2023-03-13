using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class Stomper : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.gameObject.TryGetComponent<IDamageble>(out var damageble))
            {
                damageble.TakeDamage(damage);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.gameObject.TryGetComponent<IDamageble>(out var damageble))
            {
                damageble.TakeDamage(damage);
            }
        }
    }
}
