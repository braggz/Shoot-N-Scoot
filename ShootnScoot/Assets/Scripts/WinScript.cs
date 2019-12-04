using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject P1;
    public GameObject P2;
    private bool redWin;
    private bool blueWin;
    public GameObject Canvas;
    public GameObject RedMes;
    public GameObject BlueMes;
    
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
        blueWin = P1.GetComponent<Players>().hasWon;
        redWin = P2.GetComponent<Players>().hasWon;
        Debug.Log("P1: " + blueWin);
        Debug.Log("P2: " + redWin);
        if (blueWin)
        {
            Canvas.SetActive(true);
            RedMes.SetActive(false);
            BlueMes.SetActive(true);
        }
        else if (redWin)
        {
            Canvas.SetActive(true);
            RedMes.SetActive(true);
            BlueMes.SetActive(false);
        }
    }
}
