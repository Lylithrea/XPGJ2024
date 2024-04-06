using DG.Tweening;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour, ICardInteractable
{
    [SerializeField] Transform[] _cardSpots;
    [SerializeField] Transform _circleCenter;
    [SerializeField] TweenData _goToSpot;

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

    public void Interact(GameObject card)
    {
        var openSpot = GetOpenSpot();

        card.transform.SetParent(openSpot);
        card.transform.DOMove(openSpot.position, _goToSpot.Duration).SetEase(_goToSpot.Easing);
        card.transform.DORotate(openSpot.eulerAngles, _goToSpot.Duration).SetEase(_goToSpot.Easing);
        Destroy(card.GetComponent<Draw>());
        card.gameObject.AddComponent<HandDraw>().SetRestPosition(openSpot.position);
    }
    public bool CanInteract(GameObject card)
    {
        return _cardSpots.Any(x => x.childCount == 0);
    }
}
