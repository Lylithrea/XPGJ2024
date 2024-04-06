using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodInteraction : MonoBehaviour, ICardInteractable
{
    public bool CanInteract(GameObject card)
    {
        return true;
    }

    public void Interact(GameObject card)
    {
        card.transform.position = transform.position;
    }

}
