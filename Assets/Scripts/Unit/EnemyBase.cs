using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour
{
    private float health = 25f;
    public UnityEvent OnDie = new UnityEvent();

    public float Health
    {
        get { return health; }
    }

    public void TakeDamage(float damage)
    {
        if (health <= 0f)
        {
            return;
        }
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDie?.Invoke();
        Destroy(gameObject);
    }

}
