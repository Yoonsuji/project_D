using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAttackPlayer : MonoBehaviour
{
    public PlayerStats playerStats;
    public float attackDelay = 3f;
    private List<Transform> enemies;

    private void Start()
    {
        enemies = new List<Transform>();
        StartCoroutine(DelayedAttack());
    }

    private IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(attackDelay);

        FindAllEnemies();

        foreach (Transform enemy in enemies)
        {
            EnemyBase enemyComponent = enemy.GetComponent<EnemyBase>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(playerStats.attackDamage);
            }
        }
    }

    private void FindAllEnemies()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObject in enemyObjects)
        {
            enemies.Add(enemyObject.transform);
        }
    }
}
