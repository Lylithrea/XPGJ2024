using DG.Tweening;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] Transform[] _cardSpots;
    [SerializeField] Transform _circleCenter;

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

   
}
