using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Manager : MonoBehaviour
{
    
    public GameObject OptionMenu;
    public Scene GameScene;

    private void Start()
    {
        SoundManager.Instance.PlaySound(SoundName.Menu, name: "Menu");
    }

    public void StartGame()
    {
        
        SceneManager.LoadScene("CombatScene");
       
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
