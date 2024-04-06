using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    [SerializeField] GameObject _cardPrefab;
    [SerializeField] List<SO_Card> _cards = new List<SO_Card>();
    [SerializeField] Hand _hand;

    public ReadOnlyCollection<SO_Card> Cards { get; private set; }

    void Awake()
    {
        Cards = _cards.AsReadOnly();
    }

    public GameObject GetCard()
    {
        GameObject a = Instantiate(_cardPrefab, transform);
        a.GetComponentInChildren<CardHandler>().SetupCard(_cards[Random.Range(0, _cards.Count)]);
        a.transform.position = transform.position;

        return a;
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
