using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class TankerPlayer : MonoBehaviour
{
    public PlayerStats playerStats;
    public float detectionRadius = 5f;
    public List<Transform> enemys;
    private float nextAttackTime = 0f;

    private UnitMovement unitMovement;

    Animator animator;

    private void Start()
    {
        FindEnemy();


        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (enemys.Count == 0)
        {
            FindEnemy();
        }

        Transform closestEnemy = GetClosestPlayer();
        if (closestEnemy != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, closestEnemy.position);
            if (distanceToPlayer <= detectionRadius)
            {
                Attackenemys(closestEnemy);
            }
        }
    }

    private void FindEnemy()
    {
        enemys = new List<Transform>();
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject playerObject in playerObjects)
        {
            enemys.Add(playerObject.transform);
        }
    }

    private Transform GetClosestPlayer()
    {
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        for (int i = enemys.Count - 1; i >= 0; i--)
        {
            if (enemys[i] == null)
            {
                enemys.RemoveAt(i);
                continue;
            }
            float distanceToPlayer = Vector3.Distance(transform.position, enemys[i].position);
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestPlayer = enemys[i];
            }
        }
        return closestPlayer;
    }

    private void Attackenemys(Transform enemies)
    {
        if (Time.time >= nextAttackTime)
        {
            EnemyBase enemyComponent = enemies.GetComponent<EnemyBase>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(playerStats.attackDamage);
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + playerStats.attackCooldown;

                unitMovement.isMoving = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}