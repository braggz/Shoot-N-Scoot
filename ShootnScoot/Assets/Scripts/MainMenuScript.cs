using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public static bool InLevelSelect;
    public GameObject LevelSelectUI;
    
    public GameObject PerkPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("You go to Main Menu.");
    }

    public void LoadLevelSelectMenu()
    {
        SceneManager.LoadScene("LevelSelect");
        
        Debug.Log("You go to level select.1");
    }

    public void LoadSettingsMenu()
    {
        SceneManager.LoadScene("Settings");
        Debug.Log("You go to settings.");
    }

    public void LoadBankLevel()
    {
        SceneManager.LoadScene("Bank");
        Debug.Log("You go to Bank Level.");
    }

    public void LoadCanyon()
    {
        SceneManager.LoadScene("Canyon");
        Debug.Log("You go to Canyon Level.");
    }

    public void LoadBeta()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("Time to play the Game!");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("No man ever walks out in his own story.");
    }
    public void Player1SelectReload()
    {
        PerkPrefab.GetComponent<PerkScript>().Player1ReloadSpeed = 0.5f;
        PerkPrefab.GetComponent<PerkScript>().Player1JumpSpeed = 1;
        PerkPrefab.GetComponent<PerkScript>().Player1RunSpeed = 1;

        // test = true;
    }
    public void Player2SelectReload()
    {
        //PerkPrefab.GetComponent<PerkScript>().Player2ReloadSpeed = 0.5f;

        PerkPrefab.GetComponent<PerkScript>().Player2ReloadSpeed = 0.5f;
        PerkPrefab.GetComponent<PerkScript>().Player2JumpSpeed = 1;
        PerkPrefab.GetComponent<PerkScript>().Player2RunSpeed = 1;
    }
    public void Player1SelectRun()
    {
        PerkPrefab.GetComponent<PerkScript>().Player1ReloadSpeed = 1;
        PerkPrefab.GetComponent<PerkScript>().Player1JumpSpeed = 1;
        PerkPrefab.GetComponent<PerkScript>().Player1RunSpeed = 300f;
    }
    public void Player2SelectRun()
    {
        PerkPrefab.GetComponent<PerkScript>().Player2ReloadSpeed = 1;
        PerkPrefab.GetComponent<PerkScript>().Player2JumpSpeed = 1;
        PerkPrefab.GetComponent<PerkScript>().Player2RunSpeed = 300f;
    }
    public void Player1SelectJump()
    {
        PerkPrefab.GetComponent<PerkScript>().Player1ReloadSpeed = 1;
        PerkPrefab.GetComponent<PerkScript>().Player1JumpSpeed = 300f;
        PerkPrefab.GetComponent<PerkScript>().Player1RunSpeed = 1;
    }
    public void Player2SelectJump()
    {
        PerkPrefab.GetComponent<PerkScript>().Player2ReloadSpeed = 1;
        PerkPrefab.GetComponent<PerkScript>().Player2JumpSpeed = 300f;
        PerkPrefab.GetComponent<PerkScript>().Player2RunSpeed = 1;
    }
    

}
