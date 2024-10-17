using System.Collections.Generic;
using UnityEngine;

public class MechanicalEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public float detectionRadius = 5f;
    public List<Transform> players;
    private float nextAttackTime = 0f;
    private Animator animator;

    // Define a list of allowed objects that can damage the enemy
    public List<string> allowedDamageSources; // Add tags/names of objects that can damage this enemy

    private void Start()
    {
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
            BasicPlayer playerComponent = player.GetComponent<BasicPlayer>();
            if (playerComponent != null)
            {
                if (IsAllowedDamageSource(playerComponent))
                {
                    playerComponent.TakeDamage(enemyStats.attackDamage);
                    animator.SetTrigger("Attack");
                    Debug.Log($"{enemyStats.enemyName} attacked {player.name} for {enemyStats.attackDamage} damage!");
                }
                else
                {
                    Debug.Log("Attack ignored. This player cannot damage the MechanicalEnemy.");
                }
                nextAttackTime = Time.time + enemyStats.attackCooldown;
            }
        }
    }

    // Check if the attacker is in the allowed list
    private bool IsAllowedDamageSource(BasicPlayer player)
    {
        // Assuming player has a tag or name to identify if it can damage this enemy
        return allowedDamageSources.Contains(player.gameObject.tag) || allowedDamageSources.Contains(player.gameObject.name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
