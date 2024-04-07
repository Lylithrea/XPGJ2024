using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] Transform[] _cardSpots;
    [SerializeField] Transform _circleCenter;
    public List<GameObject> cards = new List<GameObject>();

    public ReadOnlyCollection<Transform> CardSpots { get; private set; }

    void Awake()
    {
        CardSpots = Array.AsReadOnly(_cardSpots);

        foreach (Transform t in CardSpots)
        {
            t.up = (t.position - _circleCenter.position).normalized;
        }
    }

    public Transform GetOpenSpot()
    {
        return _cardSpots.FirstOrDefault(x => x.childCount == 0);

    }

    public void UpdateHandActivity(bool isActive)
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<CardHandler>().UpdateActivity(isActive);
        }
    }

   
    public void AddCard(GameObject card)
    {
        cards.Add(card);
    }

    public void RemoveCard(GameObject card)
    {
        cards.Remove(card);
    }

    public void ClearHand()
    {
        cards.Clear();
    }

}
