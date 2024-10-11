using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayer : MonoBehaviour
{
    public PlayerStats playerStats;
    public int currentHealth;
    private HPbar healthBar;

    void Start()
    {
        currentHealth = playerStats.health;

        healthBar.SetMaxHealth(playerStats.health);
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player takes " + damage + " damage. Current health: " + currentHealth);

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        Destroy(gameObject);
    }
}
