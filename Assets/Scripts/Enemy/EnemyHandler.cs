using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHandler : MonoBehaviour
{

    public Sprite enemyBackground;

    public int maxHealth;
    public int currentHealth;

    public TextMeshProUGUI enemyName;
    public Slider healthSlider;
    public Image enemyImage;

    public int currentShield;

    SO_Enemy currentEnemy;

    public void Setup(SO_Enemy enemy)
    {
        maxHealth = enemy.maxHealth;
        currentHealth = enemy.maxHealth;

        currentEnemy = enemy;

        GameManager.Instance.SetupObjectOfInterest(currentEnemy.enemySprite, currentEnemy.enemyName);

        UpdateUI();
    }

    public void SetActive(bool isActive)
    {
        enemyImage.enabled = isActive;
        enemyName.enabled = isActive;
        healthSlider.gameObject.SetActive(isActive);
    }



    public void DoAttack()
    {
        currentShield = 0;
        Attack attack = currentEnemy.GetAttack(currentHealth);
        currentEnemy.IncreaseAttack(currentHealth);
        Debug.Log("Enemy attack for " + attack.damage + " damage!");
        GameManager.Instance.PlayerManager.TakeDamage(attack.damage);
        Heal(attack.heal);
    }

    public void AddShield(int amount)
    {
        currentShield += amount;
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

        if (currentHealth <= 0)
        {
            Debug.Log("Enemy Died");
            GameManager.Instance.EndGame();
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

    public void UpdateUI()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

    }

}
