using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public struct TweenData
{
    public float Duration;
    public Ease Easing;
}
public class DeckHandler : MonoBehaviour
{
    [SerializeField] DrawPile _drawPile;
    [SerializeField] float _drawDuration;
    [SerializeField] Hand _hand;
    [SerializeField] float _waitUntilNext;
    [SerializeField] bool test;
    [SerializeField] DiscardPile _discardPile;
    [SerializeField] TweenData _discardTween;
    [SerializeField] TweenData _goToSpot;
    [SerializeField] DragDropHandler _handDrawPrefab;
    [SerializeField] GameObject back;

    public static DeckHandler Instance;

    public List<SO_Card> Deck = new ();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;

    }
    private void Start()
    {
        foreach (SO_Card card in Deck)
        {
            _drawPile.PutCardInPile(card);
        }
    }
    void OnDestroy()
    {
        Instance = null;
    }

    void Update()
    {
        if (test)
        {
            FillHand();
            test = false;
        }

    }

    public void UpdateHandActivity(bool isActive)
    {
        _hand.UpdateHandActivity(isActive);
    }

    public bool PutCardInHand()
    {
        
        Transform openSpot = _hand.GetOpenSpot();
        if (openSpot == null) return false;

        GameObject card = _drawPile.GetCard();

        if(card == null)
        {
            foreach (var discardCard in _discardPile.DiscardedCards)
                _drawPile.PutCardInPile (discardCard);
            card = _drawPile.GetCard ();

            if (card == null)
            {
                //YOU LOST! THERE ARE NO MORE CARDS TO USE!
                
                return false;
            }
        }
        
        card.GetComponentInChildren<CardFlipper>().FlipToFront();


        card.transform.SetParent(openSpot);
        card.transform.DOMove(openSpot.position, _goToSpot.Duration).SetEase(_goToSpot.Easing);
        card.transform.DORotate(openSpot.eulerAngles, _goToSpot.Duration).SetEase(_goToSpot.Easing);
        _hand.AddCard(card);

        DragDropHandler handDraw = card.AddComponent<DragDropHandler>();

        handDraw.SetRestPosition(openSpot);
        handDraw.Copy(_handDrawPrefab);
        return true;
    }

    public void FillHand()
    {
        StartCoroutine(FillHand_Cr());
    }

    public void ClearHand()
    {
        foreach (Transform child in _hand.gameObject.transform)
        {
            Destroy(child);
        }
        _hand.ClearHand();
    }

    public void DiscardCard(GameObject card)
    {
        var draw = card.GetComponent<DragDropHandler>();
        Debug.Assert(draw != null, "You can interact with discard zone only with cards that are in the hand");

        Destroy(draw);
        card.transform.DOMove(_discardPile.transform.position, _discardTween.Duration).SetEase(_discardTween.Easing).OnComplete(()=> Destroy(card));
        var rot = card.transform.eulerAngles;
        card.transform.DORotate(new Vector3(90, rot.y, rot.z), _discardTween.Duration).SetEase(_discardTween.Easing);
        card.GetComponent<Image>().DOFade(0, _discardTween.Duration / 2).SetEase(_discardTween.Easing).SetDelay( _discardTween.Duration / 2);
        _hand.RemoveCard(card);
        //add in discard
        _discardPile.DiscardedCards.Add(card.GetComponent<CardHandler>().followerCard);

    }

    IEnumerator FillHand_Cr()
    {
        while(PutCardInHand())
        {
            yield return new WaitForSeconds(_waitUntilNext);
        }
    }


    public void SacrificeCard(GameObject card)
    {
        _hand.RemoveCard(card);
    }


}
