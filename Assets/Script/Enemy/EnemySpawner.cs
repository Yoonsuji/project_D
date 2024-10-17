using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnPosition;
    public GameObject[] enemy;
    public SplineComputer enemySpline;
    public int maxEnemyCount = 0;
    public float spawnTime = 3f;
    public LayerMask collisionLayers;

    private int currentEnemyCount = 0;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (currentEnemyCount < maxEnemyCount)
        {
            yield return new WaitForSeconds(spawnTime);

            int index = Random.Range(0, enemy.Length);
            GameObject selectedEnemy = enemy[index];

            if (spawnedEnemies.Contains(selectedEnemy))
            {
                continue;
            }

            GameObject spawnedEnemy = Instantiate(selectedEnemy, spawnPosition.transform.position, Quaternion.identity);
            UnitMovement movement = spawnedEnemy.AddComponent<UnitMovement>();
            movement.spline = enemySpline;
            movement.collisionLayers = collisionLayers;
            spawnedEnemies.Add(selectedEnemy);
            currentEnemyCount++;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spawnPosition.transform.position, 0.5f);
    }
}
