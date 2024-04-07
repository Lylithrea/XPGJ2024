using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
    public int currentShield = 0;

    public Slider healthSlider;

    readonly List<Debuff> _debuffs = new();

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

    public void AddDebuff(Debuff debuff)
    {
        _debuffs.Add(debuff);
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
        
        _debuffs.Clear();

        UpdateUI();
    }

    public void UpdateUI()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void DoAttack(GameObject follower)
    {
        GodManager.instance.UseFollower();
        CardStats cardStats = follower.GetComponent<CardHandler>().GetCardStats();
        var gameManager = GameManager.Instance;

        if(_debuffs.Any(x => x.Type == DebuffType.Weak))
            gameManager.EnemyHandler.TakeDamage(cardStats.damage/2);
        else
            gameManager.EnemyHandler.TakeDamage(cardStats.damage);

        if (_debuffs.Any(x => x.Type == DebuffType.Ailment))
            gameManager.PlayerManager.Heal(cardStats.healing/2);
        else
            gameManager.PlayerManager.Heal(cardStats.healing);


        gameManager.PlayerManager.AddShield(cardStats.shield);


        Debug.Log("Card stats: " + cardStats.draw);
        for (int i = 0; i < cardStats.draw; i++)
        {
            DeckHandler.Instance.PutCardInHand();
        }

    }
}
