using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public EnemyHandler EnemyHandler;
    public PlayerManager PlayerManager;
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
        PlayerManager.StartPlayerTurn();
        DeckHandler.Instance.FillHand();
        DeckHandler.Instance.UpdateHandActivity(true);
    }


    public void SetupEnemy()
    {
        EnemyHandler.Setup(enemies[Random.Range(0, enemies.Count)]);
    }

    public void SetEnemy(SO_Enemy enemy)
    {
        EnemyHandler.Setup(enemy);
        
    }

    public void StartGame()
    {
        //handle drawing cards
        //Animations etc?
    }

    public void UseFollower(GameObject follower)
    {
        GodManager.instance.UseFollower();
        CardStats cardStats = follower.GetComponent<CardHandler>().GetCardStats();
        EnemyHandler.TakeDamage(cardStats.damage);
        PlayerManager.Heal(cardStats.healing);
        PlayerManager.AddShield(cardStats.shield);
        for (int i = 0; i < cardStats.draw; i++)
        {
            DeckHandler.Instance.PutCardInHand();
        }
    }

}
