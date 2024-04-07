using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Manager : MonoBehaviour
{
    
    public GameObject OptionMenu;
    public Scene GameScene;

    private void Start()
    {
        SoundManager.Instance.PlaySound(SoundName.Menu);
    }

    public void StartGame()
    {
        
        SceneManager.LoadScene("CombatScene");

        SoundManager.Instance.PlayBattleMusic();

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
