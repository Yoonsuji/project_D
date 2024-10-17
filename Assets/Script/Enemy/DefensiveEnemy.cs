using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dreamteck.Splines;

public class DefensiveEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public float detectionRadius = 5f;
    public List<Transform> players;
    private float nextAttackTime = 0f;
    private UnitMovement unitMovement;
    Animator animator;

    private bool isInvincible = false;
    private float specialAbilityEndTime = 0f;
    private float nextSpecialAbilityTime = 0f;

    private void Start()
    {
        FindPlayers();
        unitMovement = GetComponent<UnitMovement>();

        if (unitMovement == null)
        {
            unitMovement = gameObject.AddComponent<UnitMovement>();
        }

        unitMovement.moveSpeed = enemyStats.moveSpeed;
        animator = GetComponentInChildren<Animator>();
        nextSpecialAbilityTime = Time.time;
    }

    private void Update()
    {
        if (isInvincible && Time.time >= specialAbilityEndTime)
        {
            DisableInvincibility();
        }

        if (Time.time >= nextSpecialAbilityTime)
        {
            ActivateInvincibility();
        }

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
            else
            {
                unitMovement.ResumeMovement();
            }
        }
        else
        {
            unitMovement.ResumeMovement();
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
            if (!isInvincible) // Check for invincibility before taking damage
            {
                BasicPlayer playerComponent = player.GetComponent<BasicPlayer>();
                if (playerComponent != null)
                {
                    playerComponent.TakeDamage(enemyStats.attackDamage);
                    animator.SetTrigger("Attack");
                    nextAttackTime = Time.time + enemyStats.attackCooldown;
                    unitMovement.isMoving = false;
                }
            }
        }
    }

    private void ActivateInvincibility()
    {
        isInvincible = true;
        specialAbilityEndTime = Time.time + enemyStats.specialAbilityDuration;
        nextSpecialAbilityTime = Time.time + enemyStats.specialAbilityCooldown;
        Debug.Log("무적상태");
    }

    private void DisableInvincibility()
    {
        isInvincible = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
