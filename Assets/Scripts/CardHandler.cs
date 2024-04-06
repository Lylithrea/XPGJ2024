using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI followerName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image followerImage;

    public int adjustedDamage;
    public int adjustedHealing;
    public int adjustedShield;
    public int adjustedDraw;



    public SO_Card followerCard;

    public void Start()
    {
        SetupCard(followerCard);
    }

    public void SetupCard(SO_Card card)
    {
        followerCard = card;



        adjustedDamage = followerCard.damage;
        adjustedHealing = followerCard.healing;
        adjustedShield = followerCard.shield;
        adjustedDraw = followerCard.draw;


        UpdateUI();
    }

    public void UpdateValues()
    {
        int adjustment = GodManager.instance.GetFavourLevel(followerCard.followingGod);
        switch (followerCard.followingGod)
        {
            case Gods.Offensive:
                adjustedDamage = followerCard.damage + adjustment;
                break;
            case Gods.Defensive:
                adjustedShield = followerCard.shield + adjustment;
                break;
            case Gods.Fertility:
                adjustedHealing = followerCard.healing + adjustment;
                break;
            case Gods.Socializing:
                adjustedDraw = followerCard.draw + adjustment;
                break;
            default:
                Debug.LogWarning("A follower is following a god that does not exist.");
                break;
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        Debug.Log("Updating UI!");
        followerName.text = followerCard.followerName;

        string descriptionText = followerCard.description;
        descriptionText = descriptionText.Replace("{damage}", adjustedDamage.ToString());
        descriptionText = descriptionText.Replace("{healing}", adjustedHealing.ToString());
        descriptionText = descriptionText.Replace("{shield}", adjustedShield.ToString());
        descriptionText = descriptionText.Replace("{draw}", adjustedDraw.ToString());


        description.text = descriptionText;
        followerImage.sprite = followerCard.followerSprite;
        GetComponent<Image>().color = GodManager.instance.GetGodStats(followerCard.followingGod).godColor;
    }



}
