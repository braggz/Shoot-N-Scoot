using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScriptC : MonoBehaviour
{
    public GameObject P1;
    public GameObject P2;
    private bool redWin;
    private bool blueWin;
    public GameObject Canvas;
    public GameObject RedMes;
    public GameObject BlueMes;
    public GameObject WinTracker;
    public Text NumBlueWins;
    public Text NumRedWins;
    public GameObject PauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        redWin = false;
        blueWin = false;
        Canvas.SetActive(false);
        RedMes.SetActive(false);
        BlueMes.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        NumBlueWins.text = WinTracker.GetComponent<WinTracking>().bluewins.ToString();
        NumRedWins.text = WinTracker.GetComponent<WinTracking>().redwins.ToString();
        blueWin = P1.GetComponent<CanyonPlayerScript>().hasWon;
        redWin = P2.GetComponent<CanyonPlayerScript>().hasWon;
        Debug.Log(redWin);
        if (blueWin)
        {

            WinTracker.GetComponent<WinTracking>().bluewins += 1;
            Canvas.SetActive(true);
            RedMes.SetActive(false);
            BlueMes.SetActive(true);
            P1.GetComponent<CanyonPlayerScript>().hasWon = false;
            //P1.GetComponent<Players>().isPaused = true;
            PauseMenu.GetComponent<PauseMenuScript>().isPaused = true;

        }
        else if (redWin)
        {
            redWin = false;
            WinTracker.GetComponent<WinTracking>().redwins += 1;

            Canvas.SetActive(true);
            RedMes.SetActive(true);
            BlueMes.SetActive(false);
            P2.GetComponent<CanyonPlayerScript>().hasWon = false;
            PauseMenu.GetComponent<PauseMenuScript>().isPaused = true;

            // P1.GetComponent<Players>().isPaused = true;
        }
    }
    public void restart()
    {
        SceneManager.LoadScene("Canyon");
        //P1.GetComponent<Players>().isPaused = ;
    }
    public void ChangeLevel()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ResetWins()
    {
        WinTracker.GetComponent<WinTracking>().bluewins = 0;
        WinTracker.GetComponent<WinTracking>().redwins = 0;
    }
}
