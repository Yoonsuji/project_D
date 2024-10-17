using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardPlayer : MonoBehaviour
{
    public PlayerStats playerStats;
    public float detectionRadius = 5f;
    public float spellDamage = 10f;
    public float spellDuration = 2f;
    public Color spellColor = new Color(0.5f, 0.5f, 1f, 1f); // Spell-like color

    private List<Transform> enemies;
    private float nextAttackTime = 0f;
    private Animator animator;

    private void Start()
    {
        FindEnemies();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (enemies.Count == 0)
        {
            FindEnemies();
        }

        Transform closestEnemy = GetClosestEnemy();
        if (closestEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, closestEnemy.position);
            if (distanceToEnemy <= detectionRadius)
            {
                AttackEnemy(closestEnemy);
            }
        }
    }

    private void FindEnemies()
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

            float distanceToEnemy = Vector3.Distance(transform.position, enemies[i].position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemies[i];
            }
        }
        return closestEnemy;
    }

    private void AttackEnemy(Transform enemy)
    {
        if (Time.time >= nextAttackTime)
        {
            EnemyBase enemyComponent = enemy.GetComponent<EnemyBase>();
            if (enemyComponent != null)
            {
                StartCoroutine(ApplySpellEffect(enemyComponent));
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + playerStats.attackCooldown;
            }
        }
    }

    private IEnumerator ApplySpellEffect(EnemyBase enemy)
    {
        SpriteRenderer[] enemySprites = enemy.GetComponentsInChildren<SpriteRenderer>();
        if (enemySprites.Length > 0)
        {
            Color originalColor = enemySprites[0].color;

            int totalDamage = Mathf.FloorToInt(spellDamage);
            int damagePerSecond = Mathf.FloorToInt(totalDamage / spellDuration);

            float elapsedTime = 0f;

            while (elapsedTime < spellDuration)
            {
                enemy.TakeDamage(damagePerSecond);

                Color currentColor = Color.Lerp(spellColor, originalColor, elapsedTime / spellDuration);

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

