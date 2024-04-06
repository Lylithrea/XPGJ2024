using DG.Tweening;
using System;
using System.Collections;
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
    [SerializeField] DiscardPile _discardSpot;
    [SerializeField] TweenData _discardTween;
    [SerializeField] TweenData _goToSpot;
    [SerializeField] DragDropHandler _handDrawPrefab;

    public static DeckHandler Instance;

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

    bool PutCardInHand()
    {
        
        Transform openSpot = _hand.GetOpenSpot();
        if (openSpot == null) return false;
        GameObject card = _drawPile.GetCard();

        Debug.Assert(card != null);
        StartCoroutine(FlipCardToFront(card)) ;


        card.transform.SetParent(openSpot);
        card.transform.DOMove(openSpot.position, _goToSpot.Duration).SetEase(_goToSpot.Easing);
        card.transform.DORotate(openSpot.eulerAngles, _goToSpot.Duration).SetEase(_goToSpot.Easing);

        DragDropHandler handDraw = card.AddComponent<DragDropHandler>();

        handDraw.SetRestPosition(openSpot.position);
        handDraw.Copy(_handDrawPrefab);
        return true;
    }

    public void FillHand()
    {
        StartCoroutine(FillHand_Cr());
    }

    public void DiscardCard(GameObject card)
    {
        var draw = card.GetComponent<DragDropHandler>();
        Debug.Assert(draw != null, "You can interact with discard zone only with cards that are in the hand");

        Destroy(draw);

        card.transform.DOMove(_discardSpot.transform.position, _discardTween.Duration).SetEase(_discardTween.Easing).OnComplete(()=> Destroy(card));
        card.GetComponent<Image>().DOFade(0, _discardTween.Duration).SetEase(_discardTween.Easing);

        _discardSpot.DiscardedCards.Add(card);
    }

    IEnumerator FillHand_Cr()
    {
        while(PutCardInHand())
        {
            yield return new WaitForSeconds(_waitUntilNext);
        }
    }

    IEnumerator FlipCardToFront(GameObject card)
    {
        float duration = 0;
        while (duration < _drawDuration)
        {
            float t = duration / _drawDuration;

            card.transform.eulerAngles = new Vector3(card.transform.eulerAngles.x, 180 * (1 - t), card.transform.eulerAngles.z);
            if (t > 0.5f)
            {
                GameObject back = card.transform.Find("Back").gameObject;
                Debug.Assert(back != null, "You don't have a Back child in card game object");
                back.SetActive(false);
            }

            yield return null;
            duration += Time.deltaTime;
        }

        card.transform.eulerAngles = new Vector3(card.transform.eulerAngles.x, 0, card.transform.eulerAngles.z);
    }
}
