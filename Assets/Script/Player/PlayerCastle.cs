using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCastle : MonoBehaviour
{
    public float maxHP = 100f;
    public float currentHP;
    public Slider hpBar;

    void Start()
    {
        currentHP = maxHP;
        UpdateHPBar();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHPBar();

        if (currentHP <= 0)
        {
            Debug.Log("게임 오버");
        }
    }

    void UpdateHPBar()
    {
        if (hpBar != null)
        {
            hpBar.value = currentHP / maxHP;
        }
    }
}
