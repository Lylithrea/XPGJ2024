using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GodHandler : MonoBehaviour, ICardInteractable
{
    public SO_God god;

    public TextMeshProUGUI godName;
    public TextMeshProUGUI godDescription;
    public Slider godFavour;
    public Image godImage;


    public void SetupGod(SO_God newGod)
    {
        god = newGod;

        godName.text = god.godName;
        godDescription.text = god.godDescription;

        this.GetComponent<Image>().color = god.godColor;

        godFavour.maxValue = god.maxFavour;
        godFavour.value = god.currentFavour;
    }


    public bool CanInteract(GameObject card)
    {
        return true;
    }

    public void Interact(GameObject card)
    {
        card.transform.position = transform.position;
        StartCoroutine(card.GetComponent<DragDropHandler>().DestroyCardAfterSeconds(1));
        SacrificeFollower(card);
    }

    


    public int GetFavourLevel()
    {
        float favourSteps = god.maxFavour / god.godLevelStats.Count;
        // Determine the index in the favourValues array based on the favor count
        int index = Mathf.FloorToInt(GodManager.instance.GetCurrentFavour(god.god) / favourSteps);

        // Ensure index is within the bounds of the favourValues array
        index = Mathf.Clamp(index, 0, god.godLevelStats.Count - 1);

        // Return the value associated with the calculated index
        return god.godLevelStats[index];
    }

    public void UpdateFavour()
    {
        godFavour.value = GodManager.instance.GetCurrentFavour(god.god);
    }


    public void SacrificeFollower(GameObject card)
    {
        Debug.Log("Sacrificed the follower...");
        GodManager.instance.SacrificeHandler(god.god, card);
        SoundManager.Instance.PlaySound(SoundName.CardSacrifice);
    }

}
