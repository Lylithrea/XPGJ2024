using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics.Contracts;

public class CardHandler : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI followerName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image followerImage;
    [SerializeField] private GameObject inactiveSprite;

    public Button RewardButton;


    public int adjustedDamage;
    public int adjustedHealing;
    public int adjustedShield;
    public int adjustedDraw;



    public SO_Card followerCard;


    public void isRewardCard(bool isReward)
    {
        RewardButton.gameObject.SetActive(isReward);
    }

    public void onClickReward()
    {
        GameManager.Instance.HandleCardReward(followerCard);
    }

    public void SetupCard(SO_Card card)
    {
        followerCard = card;



        adjustedDamage = followerCard.damage;
        adjustedHealing = followerCard.healing;
        adjustedShield = followerCard.shield;
        adjustedDraw = followerCard.draw;

        UpdateValues();

        UpdateUI();
    }


    public int GetDamage()
    {
        return adjustedDamage;
    }

    public CardStats GetCardStats()
    {
        CardStats cardStats = new CardStats();
        cardStats.damage = adjustedDamage;
        cardStats.healing = adjustedHealing;
        cardStats.shield = adjustedShield;
        cardStats.draw = adjustedDraw;
        return cardStats;
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

    public void UpdateActivity(bool isActive)
    {
        inactiveSprite.SetActive(!isActive);
    }


}

public class CardStats
{
    public int damage;
    public int healing;
    public int shield;
    public int draw;
}