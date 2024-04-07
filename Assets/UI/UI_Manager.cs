using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class UI_Manager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public GameObject RewardOptionsUI;

    public Animator RewardAnimator;
    private bool RewardOpen = false;

    private bool isPaused = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Badoinging");
            if (RewardOpen == false) 
            {
                Debug.Log("shaboing");
                OpenRewardMenu();
            }
           else
            {
                Debug.Log("hoingi");
                CloseRewardMenu();
            }
                
        }
    }

    public void ExitGame()
    {

        Application.Quit();
    }
    public void PlayRewardEnter()
    {
        RewardAnimator.Play("RewardEnter");
    }
    public void PlayRewardExit()
    {
        RewardAnimator.Play("RewardExit");
    }
    public void Resume()
    {
 
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    void Pause()
    {
       
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    public void ClosePauseMenu()
    {
        pauseMenuUI.SetActive(false);
    }

    public void ExitToMainMenu()
    {

        SceneManager.LoadScene("MainMenu");
    }

    void OpenRewardMenu()
    {
        
        PlayRewardEnter();
        
        RewardOpen = true;
    }

    void CloseRewardMenu()
    {
        PlayRewardExit();
      
        RewardOpen = false;
    }

    public void Option1() 
    {

        Debug.Log("Option1 Picked");
    }

    public void Option2()
    {
        Debug.Log("Option2 Picked");
    }

    public void Option3()
    {
        Debug.Log("Option3 Picked");
    }


    public void MainMenuMusic()
    {
        SoundManager.Instance.StopRestMusic();
        SoundManager.Instance.StopBattleMusic();
        SoundManager.Instance.PlayMenuMusic();
    }


}
