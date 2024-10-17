using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BasicPlayer : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;
    public int currentHealth;
    public Slider healthSlider;

    public float detectionRadius = 5f;
    private float nextAttackTime = 0f;

    private Animator animator;
    public List<Transform> enemies = new List<Transform>();

    private void Start()
    {
        gameObject.tag = "Player";

        currentHealth = playerStats.health;
        healthSlider.maxValue = playerStats.health;
        healthSlider.value = currentHealth;

        animator = GetComponentInChildren<Animator>();
        FindEnemies();
    }

    private void Update()
    {
        DetectAndAttackEnemy();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        playerStats.health = currentHealth;
        Debug.Log("Player takes " + damage + " damage. Current health: " + currentHealth);
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("died");
        gameObject.SetActive(false);
    }

    private void DetectAndAttackEnemy()
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
            else
            {
                animator.SetBool("Attack", false);
            }
        }
    }

    private void FindEnemies()
    {
        enemies.Clear();
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
            EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(playerStats.attackDamage);
                animator.SetBool("Attack", true);
                nextAttackTime = Time.time + playerStats.attackCooldown;
                Debug.Log("Dealt " + playerStats.attackDamage + " damage to " + enemy.name);
            }

        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
