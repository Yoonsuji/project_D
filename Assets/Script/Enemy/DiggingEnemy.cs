using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggingEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public float digDuration = 2f;
    public float digCooldown = 4f;
    public Vector3 normalScale;
    public Vector3 digScale;
    public float detectionRadius = 5f;

    private bool isUnderground = false;
    private float nextDigTime = 0f;
    private Collider2D enemyCollider;
    private List<Transform> players;
    private float nextAttackTime = 0f;
    private Animator animator;

    private void Start()
    {
        normalScale = transform.localScale;
        digScale = new Vector3(normalScale.x, normalScale.y * 0.5f, normalScale.z);
        enemyCollider = GetComponent<Collider2D>();
        FindPlayers();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Time.time >= nextDigTime)
        {
            StartCoroutine(Dig());
            nextDigTime = Time.time + digCooldown;
        }

        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer != null && !isUnderground)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);
            if (distanceToPlayer <= detectionRadius)
            {
                AttackPlayer(closestPlayer);
            }
        }
    }

    private IEnumerator Dig()
    {
        isUnderground = true;
        transform.localScale = digScale;
        enemyCollider.enabled = false;
        yield return new WaitForSeconds(digDuration);

        isUnderground = false;
        transform.localScale = normalScale;
        enemyCollider.enabled = true;
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
            EnemyBase playerComponent = player.GetComponent<EnemyBase>();
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
