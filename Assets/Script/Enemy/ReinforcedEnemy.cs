using System.Collections.Generic;
using UnityEngine;

public class ReinforcedEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public float detectionRadius = 5f;
    public int attackBoost = 2;
    public float playerDetectionRadius = 3f;

    private List<EnemyBase> nearbyEnemies;
    private Animator animator;
    private float nextAttackTime = 0f;

    private List<Transform> players;

    private void Start()
    {
        nearbyEnemies = new List<EnemyBase>();
        players = new List<Transform>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        BoostNearbyEnemies();
        FindPlayers();
        Transform closestPlayer = GetClosestPlayer();

        if (closestPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);
            if (distanceToPlayer <= playerDetectionRadius)
            {
                AttackPlayer(closestPlayer);
            }
        }
    }

    private void BoostNearbyEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D collider in colliders)
        {
            EnemyBase nearbyEnemy = collider.GetComponent<EnemyBase>();
            if (nearbyEnemy != null && !nearbyEnemies.Contains(nearbyEnemy))
            {
                nearbyEnemy.enemyStats.attackDamage += attackBoost;
                nearbyEnemies.Add(nearbyEnemy);
            }
        }

        for (int i = nearbyEnemies.Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(transform.position, nearbyEnemies[i].transform.position) > detectionRadius)
            {
                nearbyEnemies[i].enemyStats.attackDamage -= attackBoost;
                nearbyEnemies.RemoveAt(i);
            }
        }
    }

    private void FindPlayers()
    {
        players.Clear();
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
            PlayerBase playerComponent = player.GetComponent<PlayerBase>();
            if (playerComponent != null)
            {
                playerComponent.TakeDamage(enemyStats.attackDamage);
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + enemyStats.attackCooldown;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
