using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPlayer : MonoBehaviour
{
    public PlayerStats playerStats;
    public float detectionRadius = 5f;
    public List<Transform> enemies;
    private float nextHealTime = 0f;
    Animator animator;

    private UnitMovement unitMovement;


    private void Start()
    {
        unitMovement = GetComponent<UnitMovement>();

        if (unitMovement == null)
        {
            unitMovement = gameObject.AddComponent<UnitMovement>();
        }

        unitMovement.moveSpeed = playerStats.moveSpeed;

        FindEnemies();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (enemies.Count == 0)
        {
            FindEnemies();
        }

        if (Time.time >= nextHealTime)
        {
            HealEnemies();
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

    private void HealEnemies()
    {
        foreach (Transform enemy in enemies)
        {
            if (enemy == transform)
            {
                continue;
            }
            EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
            if (enemyBase != null)
            {
                enemyBase.currentHealth = Mathf.Min(enemyBase.currentHealth + playerStats.attackDamage, playerStats.maxhealth);
                animator.SetTrigger("Attack");
                unitMovement.isMoving = false;
            }
        }
        nextHealTime = Time.time + playerStats.attackCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
