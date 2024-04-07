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

    public Image objectOfInterest;
    public TextMeshProUGUI objectOfInterestText;

    public GameObject continueButton;



    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DisableAllObjectsOfInterest();
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


    public void SetEnemy(SO_Enemy enemy)
    {
        EnemyHandler.SetActive(true);
        EnemyHandler.Setup(enemy);

    }

    public void SetupObjectOfInterest(Sprite sprite, string text)
    {
        objectOfInterest.sprite = sprite;
        objectOfInterestText.text = text;
    }

    public void DisableAllObjectsOfInterest()
    {
        EnemyHandler.SetActive(false);
        CampfireHandler.SetActive(false);
        ChestHandler.SetActive(false);
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
    }

    public void SetupChest()
    {
        ChestHandler.SetActive(true);
    }

    public void StartGame()
    {
        //handle drawing cards
        //Animations etc?
        DeckHandler.Instance.FillHand();
    }

    public void EndGame()
    {
        MapHandler.Instance.SetMapActive(true);
        DisableAllObjectsOfInterest();
    }

}
