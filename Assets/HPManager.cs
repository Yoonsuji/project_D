using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public float maxHP = 100f;
    public float currentHP;
    public Slider hpBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        UpdateHPBar();  
    }

    void UpdateHPBar()
    {
        hpBar.value = currentHP / maxHP;
    }

}
