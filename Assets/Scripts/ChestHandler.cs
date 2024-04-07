using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestHandler : MonoBehaviour
{
    public GameObject chest;
    public Sprite chestBackground;

    public void OnClick()
    {
        Debug.Log("Awesome chest");
    }


    public void SetActive(bool isActive)
    {
        chest.SetActive(isActive);
    }


}
