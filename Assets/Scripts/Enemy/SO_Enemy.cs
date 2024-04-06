using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Enemy")]
public class SO_Enemy : ScriptableObject
{
    
    public List<EnemyHealthPattern> enemyHealthPatterns = new List<EnemyHealthPattern>();

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
    public int damage;
    public int shield;
    public int heal;
}