using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPlayer : MonoBehaviour
{
    public PlayerStats playerStats;
    public float detectionRadius = 5f;
    public float poisonDamage = 5f;
    public float poisonDuration = 2f;
    public Color poisonColor = new Color(0.5f, 1f, 0.5f, 1f);

    private List<Transform> enemies;
    private float nextAttackTime = 0f;
    private Animator animator;

    private void Start()
    {
        FindEnemys();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (enemies.Count == 0)
        {
            FindEnemys();
        }

        Transform closestPlayer = GetClosestEnemy();
        if (closestPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);
            if (distanceToPlayer <= detectionRadius)
            {
                AttackPlayer(closestPlayer);
            }
        }
    }

    private void FindEnemys()
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
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, enemies[i].position);
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestEnemy = enemies[i];
            }
        }
        return closestEnemy;
    }

    private void AttackPlayer(Transform player)
    {
        if (Time.time >= nextAttackTime)
        {
            EnemyBase enemyComponent = player.GetComponent<EnemyBase>();
            if (enemyComponent != null)
            {
                StartCoroutine(ApplyPoisonEffect(enemyComponent));
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + playerStats.attackCooldown;
            }
        }
    }

    private IEnumerator ApplyPoisonEffect(EnemyBase enemy)
    {
        SpriteRenderer[] enemySprites = enemy.GetComponentsInChildren<SpriteRenderer>();
        if (enemySprites.Length > 0)
        {
            Color originalColor = enemySprites[0].color;

            int totalDamage = Mathf.FloorToInt(poisonDamage);
            int damagePerSecond = Mathf.FloorToInt(totalDamage / poisonDuration);

            float elapsedTime = 0f;

            while (elapsedTime < poisonDuration)
            {
                enemy.TakeDamage(damagePerSecond);

                Color currentColor = Color.Lerp(poisonColor, originalColor, elapsedTime / poisonDuration);

                foreach (var enemySprite in enemySprites)
                {
                    enemySprite.color = currentColor;
                }

                yield return new WaitForSeconds(1f);
                elapsedTime += 1f;
            }

            foreach (var enemySprite in enemySprites)
            {
                enemySprite.color = originalColor;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
