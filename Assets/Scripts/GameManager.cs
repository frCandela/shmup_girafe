using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GameManager (Singleton pattern)
public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    public Ship starterShip; 
    public PlayerContoller playerController; 

    //Awake is always called before any Start functions
    void Awake()
    {
        //Singleton pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //Initialisation checks
        if(!starterShip)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : no starter ship selected");
        }

        if (!playerController)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : no player controller selected");
        }

        //Initialize player
        playerController.Possess(starterShip);
    }



    //Update is called every frame.
    void Update()
    {

    }
}