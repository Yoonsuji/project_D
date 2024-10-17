using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class SuicideBomber : MonoBehaviour
{
    public EnemyStats enemyStats;
    public float explosionScaleMultiplier = 2f;
    public float explosionDuration = 0.5f;

    private UnitMovement unitMovement;

    private void Start()
    {
        unitMovement = GetComponent<UnitMovement>();

        if (unitMovement == null)
        {
            unitMovement = gameObject.AddComponent<UnitMovement>();
        }

        unitMovement.moveSpeed = enemyStats.moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Castle"))
        {
            PlayerCastle castleComponent = other.GetComponent<PlayerCastle>();
            if (castleComponent != null)
            {
                castleComponent.TakeDamage(enemyStats.explosionDamage);
                Debug.Log($"SuicideBomber dealt {enemyStats.explosionDamage} damage to the castle!");
            }
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = initialScale * explosionScaleMultiplier;
        float elapsedTime = 0f;

        while (elapsedTime < explosionDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / explosionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;

        Destroy(gameObject);
    }
}
