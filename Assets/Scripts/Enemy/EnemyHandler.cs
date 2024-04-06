using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHandler : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    public TextMeshProUGUI enemyName;
    public Slider healthSlider;
    public Image enemyImage;

    SO_Enemy currentEnemy;

    public void Setup(SO_Enemy enemy)
    {
        maxHealth = enemy.maxHealth;
        currentHealth = enemy.maxHealth;

        currentEnemy = enemy;

        enemyImage.sprite = currentEnemy.enemySprite;
        enemyName.text = currentEnemy.enemyName;

        UpdateUI();
    }


    public void DoAttack()
    {
        Attack attack = currentEnemy.GetAttack(currentHealth);
        currentEnemy.IncreaseAttack(currentHealth);
        Debug.Log("Enemy attack for " + attack.damage + " damage!");
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            Debug.Log("Enemy Died");
            GameManager.Instance.SetupEnemy();
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
