using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperPlayer : MonoBehaviour
{
    public PlayerStats playerStats;
    public float detectionRadius = 5f;
    public List<Transform> enemys;
    private float nextAttackTime = 0f;
    public GameObject bulletPrefab;
    Animator animator;
    private void Start()
    {
        FindEnemy();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (enemys.Count == 0)
        {
            FindEnemy();
        }

        Transform closestEnemy = GetClosestEnemy();
        if (closestEnemy != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, closestEnemy.position);
            if (distanceToPlayer <= detectionRadius)
            {
                AttackEnemy(closestEnemy);
            }
        }

    }

    private void FindEnemy()
    {
        enemys = new List<Transform>();
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject playerObject in enemyObjects)
        {
            enemys.Add(playerObject.transform);
        }
    }

    private Transform GetClosestEnemy()
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        for (int i = enemys.Count - 1; i >= 0; i--)
        {
            if (enemys[i] == null)
            {
                enemys.RemoveAt(i);
                continue;
            }

            float distanceToEnemy = Vector3.Distance(transform.position, enemys[i].position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemys[i];
            }
        }
        return closestEnemy;
    }

    private void AttackEnemy(Transform enemy)
    {
        if (Time.time >= nextAttackTime)
        {
            if (bulletPrefab != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Vector3 direction = (enemy.position - transform.position).normalized;
                float bulletSpeed = 5f;
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                animator.SetFloat("AttackState", 0.5f);
                animator.SetBool("Attack", true);
                if (bulletRb != null)
                {
                    bulletRb.velocity = direction * bulletSpeed;
                    bulletRb.gravityScale = 0;
                }

            }
            nextAttackTime = Time.time + playerStats.attackCooldown;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void DamageBullet(Transform enemys)
    {
        if (Time.time >= nextAttackTime)
        {
            EnemyBase enemyComponent = enemys.GetComponent<EnemyBase>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(playerStats.attackDamage);
            }
        }
    }
}
