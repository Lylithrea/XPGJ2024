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
        DeckHandler.Instance.DiscardCard(card);


        GameManager.Instance.UseFollower(card);
        //flipper.FlipToBack();
    }

}
