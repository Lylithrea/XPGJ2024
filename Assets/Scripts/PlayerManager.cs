using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
    public int currentShield = 0;

    public Slider healthSlider;



    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        // Subtract from shield first
        if (currentShield > 0)
        {
            currentShield -= amount;

            // If shield is depleted, reduce the remaining damage from health
            if (currentShield < 0)
            {
                amount = Mathf.Abs(currentShield);
                currentShield = 0;
            }
            else
            {
                UpdateUI();
                return;
            }
        }

        // Subtract from health
        currentHealth -= amount;

        // Check if player died
        if (currentHealth <= 0)
        {
            Debug.Log("Player died");
        }
        UpdateUI();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateUI();
    }

    public void AddShield(int amount)
    {
        currentShield += amount;
    }


    public void StartPlayerTurn()
    {
        currentShield = 0;

        UpdateUI();
    }

    public void UpdateUI()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

}
