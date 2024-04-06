using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Enemy")]
public class SO_Enemy : ScriptableObject
{
    public string enemyName;
    public Sprite enemySprite;
    public int maxHealth;



    public List<EnemyHealthPattern> enemyHealthPatterns = new List<EnemyHealthPattern>();

    public int currentHealthCatagory;
    public int currentPattern;
    public int currentAttack;

    public Attack GetAttack(float currentHealth)
    {

        CheckHealthCatagory(currentHealth);
        return enemyHealthPatterns[currentHealthCatagory].attackPattern[currentPattern].attackList[currentAttack];
    }

    public void IncreaseAttack(float currentHealth)
    {
        CheckHealthCatagory(currentHealth);
        currentAttack++;
        if (enemyHealthPatterns[currentHealthCatagory].attackPattern[currentPattern].attackList.Count >= currentAttack)
        {
            currentAttack = 0;
            currentPattern = UnityEngine.Random.Range(0, enemyHealthPatterns[currentHealthCatagory].attackPattern.Count);
        }
    }

    public void CheckHealthCatagory(float currentHealth)
    {
        float percentage = currentHealth / maxHealth * 100;
        for(int i = 0; i < enemyHealthPatterns.Count; i++)
        {
            if (enemyHealthPatterns[i].healthPercentage > percentage)
            {
                //this is where we are currently
                if (currentHealthCatagory != i)
                {
                    //enemy has changed health, changing catagory...
                    currentHealthCatagory = i;
                    currentPattern = UnityEngine.Random.Range(0, enemyHealthPatterns[currentHealthCatagory].attackPattern.Count);
                    currentAttack = 0;
                }
            }
        }
    }

}

[Serializable]
public class EnemyHealthPattern
{
    public int healthPercentage;
    public List<AttackPattern> attackPattern = new List<AttackPattern>();
}

[Serializable]
public class AttackPattern
{
    public List<Attack> attackList = new List<Attack>();
}

[Serializable]
public class Attack
{
    public Sprite attackIndication;
    public int damage;
    public int shield;
    public int heal;
}