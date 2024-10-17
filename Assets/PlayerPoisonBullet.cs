using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoisonBullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;
    public float poisonDuration = 3f;
    public Color poisonColor = Color.green;
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemyComponent = other.GetComponent<EnemyBase>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(damage);
                StartCoroutine(ApplyPoisonEffect(enemyComponent));
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplyPoisonEffect(EnemyBase enemy)
    {
        SpriteRenderer[] enemySprites = enemy.GetComponentsInChildren<SpriteRenderer>();
        if (enemySprites.Length > 0)
        {
            Color originalColor = enemySprites[0].color;

            foreach (var enemySprite in enemySprites)
            {
                enemySprite.color = poisonColor;
            }

            float originalSpeed = enemy.enemyStats.moveSpeed;
            enemy.enemyStats.moveSpeed *= 0.5f;

            yield return new WaitForSeconds(poisonDuration);

            foreach (var enemySprite in enemySprites)
            {
                enemySprite.color = originalColor;
            }
            enemy.enemyStats.moveSpeed = originalSpeed;
        }
    }
}
