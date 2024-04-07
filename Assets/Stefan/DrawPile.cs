using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    [SerializeField] GameObject _cardPrefab;
    [SerializeField] Hand _hand;
    List<SO_Card> _cards = new List<SO_Card>();



    public ReadOnlyCollection<SO_Card> Cards { get; private set; }

    void Awake()
    {
        Cards = _cards.AsReadOnly();
    }

    public void PutCardInPile( SO_Card card)
    {
        _cards.Add(card);
        this.gameObject.GetComponent<PileSize>().AdjustSize(_cards.Count);
    }

    public GameObject GetCard()
    {

        var card = _cards[Random.Range(0, _cards.Count)];

        if (card == null) return null;
        
        GameObject cardObj = Instantiate(_cardPrefab, transform);
        cardObj.GetComponentInChildren<CardHandler>().SetupCard(card);
        cardObj.transform.position = transform.position;
        _cards.Remove(card);
        //this.gameObject.GetComponent<PileSize>().AdjustSize(_cards.Count);
        return cardObj;
    }

    public GameObject GetCard(SO_Card card)
    {
        GameObject a = Instantiate(_cardPrefab, transform);
        a.GetComponentInChildren<CardHandler>().SetupCard(card);
        a.transform.position = transform.position;

        return a;
    }

    public bool HasBackup()
    {
        return transform.childCount > 1;
    }

}
