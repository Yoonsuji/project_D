using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Dreamteck.Splines.Primitives;

public class FlyingEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public GameObject bulletPrefab;
    public float detectionRadius = 5f;
    private Transform castle;
    private float nextAttackTime = 0f;

    private void Start()
    {
        FindCastle();
    }

    private void Update()
    {
        if (castle == null)
        {
            FindCastle();
            return;
        }

        float distanceToCastle = Vector3.Distance(transform.position, castle.position);
        if (distanceToCastle <= detectionRadius)
        {
            AttackCastle();
        }
    }

    private void FindCastle()
    {
        GameObject castleObject = GameObject.FindGameObjectWithTag("Castle");
        if (castleObject != null)
        {
            castle = castleObject.transform;
        }
    }

    private void AttackCastle()
    {
        if (Time.time >= nextAttackTime)
        {
            ShootBullet();
            nextAttackTime = Time.time + enemyStats.attackCooldown;
        }
    }

    private void ShootBullet()
    {
        if (bulletPrefab != null)
        {
            Vector3 BulletSpawn = transform.position + new Vector3(0, 1.5f, 0);
            GameObject bullet = Instantiate(bulletPrefab, BulletSpawn, Quaternion.identity);

            Vector3 direction = (castle.position - transform.position).normalized;

            float bulletSpeed = 5f;
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            if (bulletRb != null)
            {
                bulletRb.velocity = direction * bulletSpeed;
                bulletRb.gravityScale = 0;
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
