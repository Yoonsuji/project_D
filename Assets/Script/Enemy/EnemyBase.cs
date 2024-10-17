using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public EnemyStats enemyStats;
    public Slider hpBarSlider;
    public float currentHealth;
    public void Start()
    {
        currentHealth = enemyStats.maxhealth;
        hpBarSlider.maxValue = enemyStats.maxhealth;
        hpBarSlider.value = currentHealth;
    }

    public void Update()
    {
        hpBarSlider.value = currentHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        hpBarSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
