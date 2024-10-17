using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttackEnemy : MonoBehaviour
{
    public EnemyStats enemyStats; // Reference to the enemy stats
    public float detectionRadius = 5f; // Radius to detect players
    public float poisonDamage = 5f; // Total damage to deal over time
    public float poisonDuration = 2f; // Duration of the poison effect
    public Color poisonColor = new Color(0.5f, 1f, 0.5f, 1f); // Poison-like color

    private List<Transform> players; // List of players detected
    private float nextAttackTime = 0f; // Time until the next attack
    private Animator animator; // Animator for attack animations

    private void Start()
    {
        FindPlayers();
        animator = GetComponentInChildren<Animator>(); // Get Animator if needed
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
                StartCoroutine(ApplyPoisonEffect(playerComponent)); // Start poison effect
                animator.SetTrigger("Attack"); // Trigger attack animation
                nextAttackTime = Time.time + enemyStats.attackCooldown; // Cooldown for next attack
            }
        }
    }

    private IEnumerator ApplyPoisonEffect(BasicPlayer player)
    {
        // Get all SpriteRenderer components in the player's hierarchy
        SpriteRenderer[] playerSprites = player.GetComponentsInChildren<SpriteRenderer>();
        if (playerSprites.Length > 0)
        {
            Color originalColor = playerSprites[0].color; // Store the original color of the first SpriteRenderer

            // Calculate total damage as an integer
            int totalDamage = Mathf.FloorToInt(poisonDamage);
            int damagePerSecond = Mathf.FloorToInt(totalDamage / poisonDuration); // Damage to apply each second

            float elapsedTime = 0f; // Track elapsed time

            while (elapsedTime < poisonDuration)
            {
                // Apply damage each second
                player.TakeDamage(damagePerSecond);

                // Interpolate the color based on remaining time
                Color currentColor = Color.Lerp(poisonColor, originalColor, elapsedTime / poisonDuration);

                // Apply the color change to all SpriteRenderer components
                foreach (var playerSprite in playerSprites)
                {
                    playerSprite.color = currentColor; // Change to the current color
                }

                // Wait for one second before applying the next damage
                yield return new WaitForSeconds(1f);
                elapsedTime += 1f; // Increment elapsed time by 1 second
            }

            // Ensure the final color is reset to the original color for all sprites
            foreach (var playerSprite in playerSprites)
            {
                playerSprite.color = originalColor; // Reset the player's color
            }
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
