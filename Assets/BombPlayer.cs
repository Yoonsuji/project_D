using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlayer : MonoBehaviour
{
    public PlayerStats playerStats; // Player stats reference
    public float explosionScaleMultiplier = 2f; // How much bigger the player gets during explosion
    public float explosionDuration = 0.5f; // How long the explosion takes

    private UnitMovement unitMovement; // Reference to player movement

    private void Start()
    {
        unitMovement = GetComponent<UnitMovement>();

        if (unitMovement == null)
        {
            unitMovement = gameObject.AddComponent<UnitMovement>();
        }

        unitMovement.moveSpeed = playerStats.moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemyComponent = other.GetComponent<EnemyBase>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(playerStats.attackDamage);
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

