using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public PlayerStats playerStats;
    public float detectionRadius = 5f;
    public List<Transform> enemies;
    private float nextAttackTime = 0f;
    private Animator animator;
    private SpriteRenderer[] playerSpriteRenderers;
    private bool isStealthed = false;

    private void Start()
    {
        FindPlayers();
        animator = gameObject.GetComponentInChildren<Animator>();

        playerSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        StartCoroutine(StealthySkill());
    }

    private void Update()
    {
        if (enemies.Count == 0)
        {
            FindPlayers();
        }

        Transform closestenemy = GetClosestEnemy();
        if (closestenemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, closestenemy.position);
            if (distanceToEnemy <= detectionRadius)
            {
                AttackEnemy(closestenemy);
            }
        }
    }

    private void FindPlayers()
    {
        enemies = new List<Transform>();
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObject in enemyObjects)
        {
            enemies.Add(enemyObject.transform);
        }
    }

    private Transform GetClosestEnemy()
    {
        Transform closestEenemy = null;
        float closestDistance = Mathf.Infinity;
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            float distanceToEnemy = Vector3.Distance(transform.position, enemies[i].position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEenemy = enemies[i];
            }
        }
        return closestEenemy;
    }

    private void AttackEnemy(Transform enemy)
    {
        if (Time.time >= nextAttackTime)
        {
            EnemyBase enemyComponent = enemy.GetComponent<EnemyBase>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(playerStats.attackDamage);
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + playerStats.attackCooldown;
            }
        }
    }

    private IEnumerator StealthySkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            if (!isStealthed)
            {
                StealthOn();
                float stealthDuration = Random.Range(1f, 5f);
                yield return new WaitForSeconds(stealthDuration);
                StealthOff();
            }
        }
    }

    private void StealthOn()
    {
        foreach (SpriteRenderer spriteRenderer in playerSpriteRenderers)
        {
            if (spriteRenderer != null)
            {
                isStealthed = true;
                Color color = spriteRenderer.color;
                color.a = 0.0f;
                spriteRenderer.color = color;
            }
        }
    }

    private void StealthOff()
    {
        foreach (SpriteRenderer spriteRenderer in playerSpriteRenderers)
        {
            if (spriteRenderer != null)
            {
                isStealthed = false;
                Color color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
