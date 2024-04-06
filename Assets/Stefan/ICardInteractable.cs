using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardInteractable 
{
    bool CanInteract(GameObject card);
    void Interact(GameObject card);
}
