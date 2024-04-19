using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    public EnemyHandler EnemyHandler;
    public PlayerManager PlayerManager;
    public List<SO_Enemy> enemies = new List<SO_Enemy>();

    public static GameManager Instance;

    public Image background;


    public ChestHandler ChestHandler;
    public CampfireHandler CampfireHandler;

    public GameObject continueButton;

    public int turnDrawCards = 4;

    public GameObject EndScreen;
    public List<GameObject> victoryElements = new List<GameObject>();
    public List<GameObject> defeatElements = new List<GameObject>();

    public bool isBoss = false;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DisableAllObjectsOfInterest();
    }


    public void ShowEndScreen(bool victory)
    {
        EndScreen.SetActive(true);
        if (victory)
        {
            foreach (GameObject element in defeatElements)
            {
                element.SetActive(false);
            }
            foreach (GameObject element in victoryElements)
            {
                element.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject element in victoryElements)
            {
                element.SetActive(false);
            }
            foreach (GameObject element in defeatElements)
            {
                element.SetActive(true);
            }
        }
    }


    public void EndTurn()
    {
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
        for (int i = 0; i < turnDrawCards; i++)
        {
            DeckHandler.Instance.PutCardInHand();
        }
        DeckHandler.Instance.UpdateHandActivity(true);
    }


    public void SetEnemy(SO_Enemy enemy)
    {
        EnemyHandler.SetActive(true);
        EnemyHandler.Setup(enemy);

    }


    public void DisableAllObjectsOfInterest()
    {
        if(EnemyHandler != null)
            EnemyHandler.SetActive(false);

        if(CampfireHandler != null)
            CampfireHandler.SetActive(false);

        if(ChestHandler != null)
            ChestHandler.SetActive(false);

        if(continueButton != null)
            continueButton.SetActive(false);
    }

    public void OnClickContinue()
    {
        EndGame();
    }


    public void HandleCardReward(SO_Card card)
    {
        ChestHandler.ClosePopup();
        //add card to deck
        DeckHandler.Instance.Deck.Add(card);
        continueButton.SetActive(true);
    }

    public void EnableContinueButton()
    {
        continueButton.SetActive(true);
    }

    public void SetupCampfire()
    {
        CampfireHandler.SetActive(true);
        SoundManager.Instance.StopBattleMusic();
        SoundManager.Instance.StopSound("Menu");
        SoundManager.Instance.PlaySound(SoundName.Rest, name: "Rest");

    }

    public void SetupChest()
    {
        ChestHandler.SetActive(true);
        SoundManager.Instance.StopSound("Menu");
        SoundManager.Instance.StopSound("Rest");
        SoundManager.Instance.PlaySound(SoundName.Rest, name: "Rest");

    }

    public void StartGame()
    {
        //handle drawing cards
        //Animations etc?
        FillHand();

        SoundManager.Instance.StopSound("Menu");
        SoundManager.Instance.StopSound("Rest");

        SoundManager.Instance.PlayBattleMusic();
    }

    public void EndGame()
    {
        MapHandler.Instance.SetMapActive(true);
        SoundManager.Instance.StopSound("Rest");
        SoundManager.Instance.StopBattleMusic();
        SoundManager.Instance.PlaySound(SoundName.Menu, name: "Menu");
        DisableAllObjectsOfInterest();
    }

}
