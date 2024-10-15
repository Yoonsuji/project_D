using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BasicPlayer : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;
    public int currentHealth;
    public Slider healthSlider;

    private void Start()
    {
        gameObject.tag = "Player";

        currentHealth = playerStats.health;
        healthSlider.maxValue = playerStats.health;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        playerStats.health = currentHealth;
        Debug.Log("Player takes " + damage + " damage. Current health: " + currentHealth);
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("died");
        gameObject.SetActive(false);
    }
}