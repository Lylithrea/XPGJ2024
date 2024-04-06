using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public EnemyHandler EnemyHandler;
    public List<SO_Enemy> enemies = new List<SO_Enemy>();

    public static GameManager Instance;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        SetupEnemy();
    }

    public void EndTurn()
    {
        Debug.Log("Ending turn");
        DeckHandler.Instance.UpdateHandActivity(false);
        HandleEnemyTurn();
        Invoke(nameof(FillHand), 2f);
    }


    public void HandleEnemyTurn()
    {
        EnemyHandler.DoAttack();
    }

    public void FillHand()
    {
        DeckHandler.Instance.FillHand();
        DeckHandler.Instance.UpdateHandActivity(true);
    }


    public void SetupEnemy()
    {
        EnemyHandler.Setup(enemies[Random.Range(0, enemies.Count)]);
    }

    public void UseFollower(GameObject follower)
    {
        GodManager.instance.UseFollower();
        int damage = follower.GetComponent<CardHandler>().GetDamage();
        EnemyHandler.TakeDamage(damage);
    }

}
