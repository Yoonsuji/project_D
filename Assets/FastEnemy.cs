using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class FastEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public float detectionRadius = 5f;
    public List<Transform> players;
    private float nextAttackTime = 0f;

    private UnitMovement unitMovement;

    Animator animator;

    private void Start()
    {
        FindPlayers();

        unitMovement = GetComponent<UnitMovement>();

        if (unitMovement == null)
        {
            unitMovement = gameObject.AddComponent<UnitMovement>();
        }

        unitMovement.moveSpeed = enemyStats.moveSpeed;

        animator = GetComponentInChildren<Animator>();
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
            else
            {
                unitMovement.ResumeMovement();
            }
        }
        else
        {
            unitMovement.ResumeMovement();
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
            BasicPlayer playerComponent = player.GetComponent<BasicPlayer>();
            if (playerComponent != null)
            {
                playerComponent.TakeDamage(enemyStats.attackDamage);
                animator.SetTrigger("Attack");
                Debug.Log($"{enemyStats.enemyName} attacked {player.name} for {enemyStats.attackDamage} damage!");
                nextAttackTime = Time.time + enemyStats.attackCooldown;

                unitMovement.isMoving = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}