using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodManager : MonoBehaviour
{

    public List<Transform> godPositions = new List<Transform>();
    public List<SO_God> gods = new List<SO_God>();
    public Dictionary<Gods, GodHandler> godHandlers = new Dictionary<Gods, GodHandler>();
    public GameObject godPrefab;

    private Dictionary<Gods, GodFavour> godFavours = new Dictionary<Gods, GodFavour>();

    public static GodManager instance;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        godFavours.Clear();
        godHandlers.Clear();

        for (int i = 0; i < gods.Count; i++)
        {
            if(i >= godPositions.Count) { Debug.LogWarning("There are more gods than positions for them."); return; }
            GameObject newGod = Instantiate(godPrefab, godPositions[i]);
            //newGod.transform.position = godPositions[i].transform.position;
            
            godHandlers.Add(gods[i].god, newGod.GetComponentInChildren<GodHandler>());   
            godFavours.Add(gods[i].god, new GodFavour(gods[i], gods[i].currentFavour));
            newGod.GetComponentInChildren<GodHandler>().SetupGod(gods[i]);
        }
    }


    public Gods addGod;
    public int amount;
    [Button]
    public void AddFavourToGod()
    {
        AdjustFavour(addGod, amount);
    }


    public void UseFollower()
    {
        foreach (KeyValuePair<Gods, GodFavour> god in godFavours)
        {
            AdjustFavour(god.Key, -1);
        }


    }


    public void SacrificeHandler(Gods god, GameObject follower)
    {
        if (follower.GetComponent<CardHandler>().followerCard.followingGod == god)
        {
            AdjustFavour(god, 25);
        }
        else
        {
            AdjustFavour(god, 15);
        }

        switch (god)
        {
            case Gods.Offensive:
                AdjustFavour(Gods.Defensive, -10);
                break;
            case Gods.Defensive:
                AdjustFavour(Gods.Offensive, -10);
                break;
            case Gods.Fertility:
                AdjustFavour(Gods.Socializing, -10);
                break;
            case Gods.Socializing:
                AdjustFavour(Gods.Fertility, -10);
                break;
            default:
                Debug.LogWarning("God does not exist.");
                break;
        }

        DeckHandler.Instance.Deck.Remove(follower.GetComponent<CardHandler>().followerCard);
    }


    public int GetCurrentFavour(Gods god)
    {
        if (!godFavours.ContainsKey(god)) { Debug.LogWarning("Something went wrong, there is no favour for this god."); return 0; }
        return godFavours[god].currentFavour;
    }

    public int GetFavourLevel(Gods god)
    {
        if (!godHandlers.ContainsKey(god)) { Debug.LogWarning("Something went wrong, there is no handler for this god."); return 0; }
        return godHandlers[god].GetFavourLevel();
    }

    public void AdjustFavour(Gods god, int amount)
    {
        if (!godFavours.ContainsKey(god)) { Debug.LogWarning("Something went wrong, there is no favour for this god."); return; }
        godFavours[god].currentFavour += amount;
        godHandlers[god].UpdateFavour();
        BadUpdateOfCards();
    }

    public SO_God GetGodStats(Gods god)
    {
        if (!godFavours.ContainsKey(god)) { Debug.LogWarning("Something went wrong, there is no stats for this god."); return null; }
        return godFavours[god].god;
    }

    public void BadUpdateOfCards()
    {
        Debug.LogWarning("Please remove this later, updating the cards the bad way");
        CardHandler[] cards = UnityEngine.Object.FindObjectsOfType<CardHandler>();
        foreach (CardHandler card in cards)
        {
            card.UpdateValues();
        }
    }

}


public class GodFavour
{
    public SO_God god;
    public int currentFavour;

    public GodFavour(SO_God god, int currentFavour)
    {
        this.god = god;
        this.currentFavour = currentFavour;
    }

}