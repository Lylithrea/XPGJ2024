using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CardAutoDragger : MonoBehaviour
{
    [SerializeField] Deck _deck;
    [SerializeField] Hand _hand;
    [SerializeField] float _waitUntilNext;
    [SerializeField] bool test;
    private void Update()
    {
        if (test)
        {
            FillHand();
            test = false;
        }
    }
    public bool PutCardInHand()
    {

        Draw card = _deck.GetCard();

        if (!_hand.CanInteract(card.gameObject)) return false;

        Debug.Assert(card != null);
        card.GetComponent<CardFlipper>().FlipToFront();
        
        _hand.Interact(card.gameObject);
        return true;
    }

    public void FillHand()
    {
        StartCoroutine(FillHand_Cr());
    }

    IEnumerator FillHand_Cr()
    {
        while(PutCardInHand())
        {
            yield return new WaitForSeconds(_waitUntilNext);
        }
    }
}
