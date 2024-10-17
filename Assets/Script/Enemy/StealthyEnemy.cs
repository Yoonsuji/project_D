using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthyEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public float detectionRadius = 5f;
    public List<Transform> players;
    private float nextAttackTime = 0f;
    private Animator animator;
    private SpriteRenderer[] enemySpriteRenderers;
    private bool isStealthed = false;

    private void Start()
    {
        FindPlayers();
        animator = gameObject.GetComponentInChildren<Animator>();

        enemySpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        StartCoroutine(StealthySkill());
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
                playerComponent.TakeDamage(enemyStats.attackDamage);
                animator.SetTrigger("Attack");
                Debug.Log($"{enemyStats.enemyName} attacked {player.name} for {enemyStats.attackDamage} damage!");
                nextAttackTime = Time.time + enemyStats.attackCooldown;
            }
        }
    }

    private IEnumerator StealthySkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            if (!isStealthed)
            {
                StealthOn();
                float stealthDuration = Random.Range(1f, 5f);
                yield return new WaitForSeconds(stealthDuration);
                StealthOff();
            }
        }
    }

    // Makes the enemy semi-transparent
    private void StealthOn()
    {
        foreach (SpriteRenderer spriteRenderer in enemySpriteRenderers)
        {
            if (spriteRenderer != null)
            {
                isStealthed = true;
                Color color = spriteRenderer.color;
                color.a = 0.3f;
                spriteRenderer.color = color;
            }
        }
        Debug.Log($"{enemyStats.enemyName} has become partially transparent!");
    }

    private void StealthOff()
    {
        foreach (SpriteRenderer spriteRenderer in enemySpriteRenderers)
        {
            if (spriteRenderer != null)
            {
                isStealthed = false;
                Color color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }
        }
        Debug.Log($"{enemyStats.enemyName} is now fully opaque!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
