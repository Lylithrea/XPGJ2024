using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireHandler : MonoBehaviour
{
    public GameObject campfire;
    public Sprite campfireBackground;

    
    public void Setup()
    {

    }

    public void OnClick()
    {
        Debug.Log("Beautiful campfire it is");
        GameManager.Instance.PlayerManager.Heal(GameManager.Instance.PlayerManager.maxHealth / 4);
        GameManager.Instance.EnableContinueButton();
    }

    public void SetActive(bool isActive)
    {
        campfire.SetActive(isActive);
    }



}
