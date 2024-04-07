using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DebuffType
{
    Weak,
    Ailment
}

public struct Debuff
{
    public DebuffType Type;
    public float value;

    public Debuff(DebuffType type, float value)
    {
        Type = type;
        this.value = value;
    }
}

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

    readonly List<Debuff> _debuffs = new ();

    public void Setup(SO_Enemy enemy)
    {
        maxHealth = enemy.maxHealth;
        currentHealth = enemy.maxHealth;

        currentEnemy = enemy;

        enemyImage.sprite = currentEnemy.enemySprite;
        enemyName.text = currentEnemy.enemyName;

        UpdateUI();
    }

    public void SetActive(bool isActive)
    {
        enemyImage.enabled = isActive;
        enemyName.enabled = isActive;
        healthSlider.gameObject.SetActive(isActive);
    }

    public void AddDebuff(Debuff debuff)
    {
        _debuffs.Add(debuff);
    }

    public void DoAttack()
    {
        currentShield = 0;
        Attack attack = currentEnemy.GetAttack(currentHealth);
        currentEnemy.IncreaseAttack(currentHealth);

        
        if(_debuffs.Any(x => x.Type == DebuffType.Weak))
        {
            Debug.Log("Enemy attack for " + attack.damage + " damage!");
            GameManager.Instance.PlayerManager.TakeDamage(attack.damage / 2);

        }
        else
        {
            Debug.Log("Enemy attack for " + attack.damage + " damage!");
            GameManager.Instance.PlayerManager.TakeDamage(attack.damage);
        }

        if (_debuffs.Any(x => x.Type == DebuffType.Weak))
            Heal(attack.heal / 2);
        else
            Heal(attack.heal);

        _debuffs.Clear();
    }

    public void AddShield(int amount)
    {
        currentShield += amount;
    }

    public void TakeDamage(int amount)
    {
        SoundManager.Instance.PlaySound(SoundName.EnemyHit);
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
