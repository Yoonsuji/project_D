using System.Collections.Generic;
using UnityEngine;

public class BerserkerEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    private EnemyBase enemyBase;

    public float detectionRadius = 5f;
    public List<Transform> players;

    private float nextAttackTime = 0f;
    private Animator animator;

    private void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        if (enemyBase == null)
        {
            return;
        }

        FindPlayers();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (players.Count == 0)
        {
            FindPlayers();
        }

        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);
            if (distanceToPlayer <= detectionRadius)
            {
                AttackPlayer(closestPlayer);
            }
        }
    }

    private void FindPlayers()
    {
        players = new List<Transform>();
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObject in playerObjects)
        {
            players.Add(playerObject.transform);
        }
    }

    private Transform GetClosestPlayer()
    {
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        for (int i = players.Count - 1; i >= 0; i--)
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
                continue;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, players[i].position);
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestPlayer = players[i];
            }
        }
        return closestPlayer;
    }

    private void AttackPlayer(Transform player)
    {
        if (Time.time >= nextAttackTime)
        {
            EnemyBase playerComponent = player.GetComponent<EnemyBase>();
            if (playerComponent != null)
            {
                int attackDamage = (int)enemyStats.attackDamage;

                if (enemyBase.currentHealth <= enemyStats.maxhealth * 0.5f)
                {
                    attackDamage *= 2;
                }

                playerComponent.TakeDamage(attackDamage);
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + enemyStats.attackCooldown;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
