using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class HealingEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
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

        unitMovement.moveSpeed = enemyStats.moveSpeed;

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
                enemyBase.currentHealth = Mathf.Min(enemyBase.currentHealth + enemyStats.healAmount, enemyStats.maxhealth);
                animator.SetTrigger("Attack");
                Debug.Log($"{enemyStats.enemyName} healed {enemy.name} for {enemyStats.healAmount} health!");
                unitMovement.isMoving = false;
            }
        }
        nextHealTime = Time.time + enemyStats.specialAbilityCooldown;
    }
}
