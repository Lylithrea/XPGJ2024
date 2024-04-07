using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public List<SO_Card> allCards = new List<SO_Card>();

    public static CardManager Instance;
    public GameObject cardPrefab;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public SO_Card GetRandomCard()
    {
        int random = Random.Range(0, allCards.Count);
        return allCards[random];
    }

}
