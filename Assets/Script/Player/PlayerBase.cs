using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    public PlayerStats playerStats;
    public Slider hpBarSlider;
    public float currentHealth;
    public void Start()
    {
        currentHealth = playerStats.maxhealth;
        hpBarSlider.maxValue = playerStats.maxhealth;
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
