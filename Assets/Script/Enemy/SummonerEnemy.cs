using System.Collections.Generic;
using UnityEngine;

public class SummonerEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public GameObject summonPrefab;
    public float detectionRadius = 5f;
    public List<Transform> players;

    private float nextSummonTime = 0f;
    private float nextAttackTime = 0f;
    private Animator animator;

    private List<GameObject> summonedUnits = new List<GameObject>();

    private void Start()
    {
        FindPlayers();
        animator = gameObject.GetComponentInChildren<Animator>();

        nextSummonTime = Time.time + 1f;
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

        if (Time.time >= nextSummonTime)
        {
            ExecuteSummonSkill();
        }

        summonedUnits.RemoveAll(unit => unit == null);
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

    private void ExecuteSummonSkill()
    {
        if (CountActiveSummonedUnits() < 3)
        {
            GameObject summonedUnit = Instantiate(summonPrefab, transform.position + Vector3.right * 0.5f, Quaternion.identity);

            summonedUnit.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);

            summonedUnits.Add(summonedUnit);

            nextSummonTime = Time.time + 0.5f;
        }
    }

    private int CountActiveSummonedUnits()
    {
        return summonedUnits.Count;
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
