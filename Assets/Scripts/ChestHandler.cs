using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestHandler : MonoBehaviour
{
    public GameObject chest;
    public Sprite chestBackground;

    public int rewardCount = 3;

    public GameObject rewardScreen;
    public GameObject gridParent;

    public void OnClick()
    {
        Debug.Log("Awesome chest");
        rewardScreen.SetActive(true);
        
        for (int i = 0; i < rewardCount; i++)
        {
            SO_Card newCard = CardManager.Instance.GetRandomCard();
            GameObject a = Instantiate(CardManager.Instance.cardPrefab, gridParent.transform);
            a.GetComponentInChildren<CardHandler>().SetupCard(newCard);
            a.GetComponentInChildren<CardHandler>().isRewardCard(true);
            a.GetComponentInChildren<CardFlipper>().FlipToFront();
        }

    }


    public void ClosePopup()
    {
        rewardScreen.SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        chest.SetActive(isActive);
    }


}
