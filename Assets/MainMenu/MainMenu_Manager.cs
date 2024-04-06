using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Manager : MonoBehaviour
{
    
    public GameObject OptionMenu;

    public void StartGame()
    {
        
        SceneManager.LoadScene("Thomas_Scene");
    }

    public void OpenOptions()
    {
        
        Debug.Log("Options menu opened.");
        OptionMenu.SetActive(true);
    }

    public void CloseOptions()
    {

        Debug.Log("Options menu opened.");
        OptionMenu.SetActive(false);
    }
    public void ExitGame()
    {
        
        Application.Quit();
    }

   


}
