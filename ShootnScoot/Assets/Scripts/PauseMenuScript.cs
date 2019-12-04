using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static bool GameIsPaused;
    public GameObject pauseMenuUI;
    // Update is called once per frame
    public bool isPaused = false;
    void Start()
    {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (GameIsPaused)
            {
                isPaused = false;
                Resume();

               
            }
            else
            {
                isPaused = true;
                Pause();
                
            }
        }
    }

    public void Resume()
    {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        
    }

    void Pause()
    {
        GameIsPaused = true;
        Debug.Log("test");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        
    }

    public void LoadLevelSelectMenu()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void LoadSettingsMenu()
    {
        SceneManager.LoadScene("Settings");
    }

    public void LoadBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}