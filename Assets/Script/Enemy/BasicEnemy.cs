using UnityEngine;
using System.Collections.Generic;

public class BasicEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public List<Transform> players; 
    public float detectionRadius = 1f;

    private float nextAttackTime = 0f;

    private void Update()
    {
        Transform closestPlayer = GetClosestPlayer();

        if (closestPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);

            if (distanceToPlayer <= detectionRadius)
            {
                AttackPlayer(closestPlayer);
                Debug.Log("플레이어 감지");
            }
        }
    }

    private Transform GetClosestPlayer()
    {
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    private void AttackPlayer(Transform player)
    {
        if (Time.time >= nextAttackTime)
        {
            BasicPlayer playerHealth = player.GetComponent<BasicPlayer>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyStats.attackDamage);
                Debug.Log($"{enemyStats.enemyName} attacked {playerHealth.name} for {enemyStats.attackDamage} damage!");
            }

            nextAttackTime = Time.time + enemyStats.attackCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
