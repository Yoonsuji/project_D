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
    private SpriteRenderer[] enemySpriteRenderers; // Array of all SpriteRenderers in this object and children
    private bool isStealthed = false; // Track stealth state

    private void Start()
    {
        FindPlayers();
        animator = gameObject.GetComponentInChildren<Animator>();

        // Get all SpriteRenderers from this object and its children (including grandchildren)
        enemySpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        StartCoroutine(StealthySkill()); // Start the stealth skill coroutine
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

    // Stealth skill coroutine: Makes the enemy partially transparent for 1-5 seconds randomly
    private IEnumerator StealthySkill()
    {
        while (true) // Loop to continuously apply the stealth effect
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f)); // Wait for a random interval before turning semi-transparent
            if (!isStealthed)
            {
                // Become semi-transparent
                StealthOn();
                float stealthDuration = Random.Range(1f, 5f); // Randomly choose a duration between 1 and 5 seconds
                yield return new WaitForSeconds(stealthDuration); // Stay semi-transparent for the random duration
                StealthOff(); // Become fully opaque again
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
                color.a = 0.3f; // Set alpha to 30% (partially transparent)
                spriteRenderer.color = color;
            }
        }
        Debug.Log($"{enemyStats.enemyName} has become partially transparent!");
    }

    // Makes the enemy fully opaque again
    private void StealthOff()
    {
        foreach (SpriteRenderer spriteRenderer in enemySpriteRenderers)
        {
            if (spriteRenderer != null)
            {
                isStealthed = false;
                Color color = spriteRenderer.color;
                color.a = 1f; // Set alpha back to 100% (fully opaque)
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
