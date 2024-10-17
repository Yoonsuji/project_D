using Dreamteck.Splines.Primitives;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SniperEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public float detectionRadius = 5f;
    public List<Transform> players;
    private float nextAttackTime = 0f;
    public GameObject bulletPrefab;
    Animator animator;
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
            if (bulletPrefab != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Vector3 direction = (player.position - transform.position).normalized;
                float bulletSpeed = 5f;
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                animator.SetFloat("AttackState", 0.5f);
                animator.SetBool("Attack", true);
                if (bulletRb != null)
                {
                    bulletRb.velocity = direction * bulletSpeed;
                    bulletRb.gravityScale = 0;
                }

            }
            nextAttackTime = Time.time + enemyStats.attackCooldown;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void DamageBullet(Transform player)
    {
        if (Time.time >= nextAttackTime)
        {
            BasicPlayer playerComponent = player.GetComponent<BasicPlayer>();
            if (playerComponent != null)
            {
                playerComponent.TakeDamage(enemyStats.attackDamage);
            }
        }
    }
}