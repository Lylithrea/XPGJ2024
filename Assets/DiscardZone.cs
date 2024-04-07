using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardZone : MonoBehaviour, ICardInteractable
{
    [SerializeField] TweenData _discardTween;

    public bool CanInteract(GameObject card)
    {
        return true;
    }

    public void Interact(GameObject card)
    {
        card.transform.SetParent(card.transform.parent.parent);
        DeckHandler.Instance.DiscardCard(card);
        GameManager.Instance.PlayerManager.DoAttack(card);




        //flipper.FlipToBack();
    }

}
