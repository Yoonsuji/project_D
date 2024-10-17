using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttackEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public float detectionRadius = 5f;
    public float poisonDamage = 5f;
    public float poisonDuration = 2f;
    public Color poisonColor = new Color(0.5f, 1f, 0.5f, 1f);

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
            PlayerBase playerComponent = player.GetComponent<PlayerBase>();
            if (playerComponent != null)
            {
                StartCoroutine(ApplyPoisonEffect(playerComponent));
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + enemyStats.attackCooldown;
            }
        }
    }

    private IEnumerator ApplyPoisonEffect(PlayerBase player)
    {
        SpriteRenderer[] playerSprites = player.GetComponentsInChildren<SpriteRenderer>();
        if (playerSprites.Length > 0)
        {
            Color originalColor = playerSprites[0].color;

            int totalDamage = Mathf.FloorToInt(poisonDamage);
            int damagePerSecond = Mathf.FloorToInt(totalDamage / poisonDuration);

            float elapsedTime = 0f;

            while (elapsedTime < poisonDuration)
            {
                player.TakeDamage(damagePerSecond);

                Color currentColor = Color.Lerp(poisonColor, originalColor, elapsedTime / poisonDuration);

                foreach (var playerSprite in playerSprites)
                {
                    playerSprite.color = currentColor;
                }

                yield return new WaitForSeconds(1f);
                elapsedTime += 1f;
            }

            foreach (var playerSprite in playerSprites)
            {
                playerSprite.color = originalColor;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
