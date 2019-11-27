using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerkScript : MonoBehaviour
{
   
    public float Player1RunSpeed = 1;
    public float Player1JumpSpeed = 1;
    public float Player1ReloadSpeed = 1;
    public float Player2RunSpeed = 1;
    public float Player2JumpSpeed = 1;
    public float Player2ReloadSpeed = 1;
   
    private void Awake()
    {
        Player1RunSpeed = 1;
        Player1JumpSpeed = 1;
        Player1ReloadSpeed = 1;
        Player2RunSpeed = 1;
        Player2JumpSpeed = 1;
        Player2ReloadSpeed = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //  test1.SetActive(true);
       
    }
    
    public void Player1SelectReload()
    {
        Player1ReloadSpeed = 0.5f;
        Player1JumpSpeed = 1;
        Player1RunSpeed = 1;
        Debug.Log("test2");
       

        // test = true;
    }
    public void Player2SelectReload()
    {
        Debug.Log("test1");
        Player2ReloadSpeed = 0.5f;
        Player2JumpSpeed = 1;
        Player2RunSpeed = 1;
    }
    public void Player1SelectRun()
    {
        Player1ReloadSpeed = 1;
        Player1JumpSpeed = 1;
        Player1RunSpeed =2f;
        
    }
    public void Player2SelectRun()
    {
        Player2ReloadSpeed = 1;
        Player2JumpSpeed = 1;
        Player2RunSpeed = 2f;
    }
    public void Player1SelectJump()
    {
        Player1ReloadSpeed = 1;
        Player1JumpSpeed = 2f;
        Player1RunSpeed = 1;
       
    }
    public void Player2SelectJump()
    {
        Player2ReloadSpeed = 1;
        Player2JumpSpeed = 2f;
        Player2RunSpeed = 1;
    }


}
