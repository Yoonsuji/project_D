using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnPosition;
    public GameObject[] enemy;
    public SplineComputer enemySpline;
    public float moveSpeed = 5f;
    public int maxEnemyCount = 0;
    public float spawnTime = 3f;

    Animator ani;
    Rigidbody2D rig;

    private int currentEnemyCount = 0;

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

            GameObject spawnedEnemy = Instantiate(enemy[index], spawnPosition.transform.position, Quaternion.identity);

            StartCoroutine(MoveAlongSpline(spawnedEnemy));
            currentEnemyCount++;
        }
    }

    IEnumerator MoveAlongSpline(GameObject enemy)
    {
        int pointCount = enemySpline.pointCount;
        

        for (int i = 0; i < pointCount - 1; i++)
        {
            Vector3 startPosition = enemySpline.GetPointPosition(i);
            Vector3 endPosition = enemySpline.GetPointPosition(i + 1);

            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * moveSpeed / Vector3.Distance(startPosition, endPosition);
                ani = enemy.GetComponent<Animator>();
                enemy.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }
        }
    }
}