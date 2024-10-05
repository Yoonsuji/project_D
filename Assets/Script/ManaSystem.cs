using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    public float maxMana = 100f; 
    public float currentMana;
    public float manaRegenRate = 0.4f;
    public Slider manaBar;


    void Start()
    {
        currentMana = maxMana;
        UpdateManaBar();
    }
    void Update()
    {
        RegenerateMana();
        UpdateManaBar();
    }

    public void OnManaButtonClick(float manaCost)
    {
        if (currentMana >= manaCost)
        {
            UseMana(manaCost);
            UpdateManaBar();
        }
        else
        {
            Debug.Log("마나가 부족합니다.");
            if(currentMana < manaCost)
            {
                Button button = GetComponent<Button>();
                button.interactable = false;
            }
            
        }
    }

    void UseMana(float amount)
    {
        currentMana -= amount;
        if (currentMana < 0)
        {
            currentMana = 0;
        }
        
    }

    void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            if (currentMana > maxMana)
            {
                currentMana = maxMana;
            }
            
        }
    }

    void UpdateManaBar()
    {
        manaBar.value = currentMana / maxMana;
    }

}
