using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public float detectionRadius = 7f;
    public float fireAttackRange = 5f;
    public float meleeAttackRange = 2f;
    public GameObject firePrefab;
    public int meleeDamage = 30;
    private List<Transform> players;
    private float nextAttackTime = 0f;
    private Animator animator;

    private void Start()
    {
        FindPlayers();
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

            if (distanceToPlayer <= meleeAttackRange)
            {
                MeleeAttackPlayer(closestPlayer);
            }
            else if (distanceToPlayer <= fireAttackRange)
            {
                FireAttackPlayer(closestPlayer);
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

    private void MeleeAttackPlayer(Transform player)
    {
        if (Time.time >= nextAttackTime)
        {
            PlayerBase playerComponent = player.GetComponent<PlayerBase>();
            if (playerComponent != null)
            {
                playerComponent.TakeDamage(meleeDamage);
                animator.SetTrigger("Attack");
                Debug.Log($"Boss dealt {meleeDamage} melee damage to {player.name}");
            }

            nextAttackTime = Time.time + enemyStats.attackCooldown;
        }
    }

    private void FireAttackPlayer(Transform player)
    {
        if (Time.time >= nextAttackTime)
        {
            if (firePrefab != null)
            {
                GameObject fire = Instantiate(firePrefab, transform.position, Quaternion.identity);
                Vector3 direction = (player.position - transform.position).normalized;

                float fireSpeed = 5f;
                Rigidbody2D fireRb = fire.GetComponent<Rigidbody2D>();
                animator.SetTrigger("Attack");

                if (fireRb != null)
                {
                    fireRb.velocity = direction * fireSpeed;
                    fireRb.gravityScale = 0;
                }

                StartCoroutine(ApplyFireDamageOverTime(player.GetComponent<PlayerBase>()));
            }

            nextAttackTime = Time.time + enemyStats.attackCooldown;
        }
    }

    private IEnumerator ApplyFireDamageOverTime(PlayerBase player)
    {
        float damageDuration = 3f;
        float damageInterval = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < damageDuration && player != null)
        {
            Bullet fire = firePrefab.GetComponent<Bullet>();
            player.TakeDamage(fire.damage);
            yield return new WaitForSeconds(damageInterval);
            elapsedTime += damageInterval;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
