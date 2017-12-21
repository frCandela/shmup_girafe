using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GameManager (Singleton pattern)
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public static Ship StarterShip { get; private set; }
    public static Controller PlayerController { get; private set; }
    public static CameraController MainCameraController { get; private set; }

    public Ship InitStarterShip;
    public Controller InitPlayerController;
    public CameraController InitMainCameraController;

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
        if(!InitStarterShip)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : no starter ship selected");
        }

        if (!InitPlayerController)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : no player controller selected");
        }

        if (!InitMainCameraController)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : no main camera selected");
        }

        StarterShip = InitStarterShip;
        PlayerController = InitPlayerController;
        MainCameraController = InitMainCameraController;

        //Initialize player
        PlayerController.Possess(StarterShip);


    }
}