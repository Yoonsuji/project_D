using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;

    public GameObject SniperPlayer;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemyComponent = other.GetComponent<EnemyBase>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
