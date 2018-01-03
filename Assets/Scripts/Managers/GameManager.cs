using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

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
    private PostProcessingBehaviour PostProcessing;

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
            //UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : no starter ship selected");
        }

        if (!InitPlayerController)
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : no player controller selected");
        }

        if (!InitMainCameraController)
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : no main camera selected");
        }

        StarterShip = InitStarterShip;
        PlayerController = InitPlayerController;
        MainCameraController = InitMainCameraController;
        PostProcessing = MainCameraController.gameObject.GetComponent<PostProcessingBehaviour>();

        //Initialize player
        PlayerController.Possess(StarterShip);
    }

    bool inHackEffect = false;
    public void ToogleHackEffect() {
        if(!inHackEffect) {
            StopCoroutine(stopHack(1f));
            StartCoroutine(startHack(1f));
        } else {
            StopCoroutine(startHack(1f));
            StartCoroutine(stopHack(1f));
        }
        inHackEffect = !inHackEffect;
    }

    IEnumerator startHack(float timing) {
        UserLutModel.Settings set = PostProcessing.profile.userLut.settings;
        
        float elapsedTime = 0;
        while (elapsedTime < timing) {
            set.contribution = Mathf.Lerp(0, 1, elapsedTime / timing);
            PostProcessing.profile.userLut.settings = set;
            elapsedTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        set.contribution = 1;
        PostProcessing.profile.userLut.settings = set;
    }

    IEnumerator stopHack(float timing) {
        UserLutModel.Settings set = PostProcessing.profile.userLut.settings;

        float elapsedTime = 0;
        while (elapsedTime < timing) {
            set.contribution = Mathf.Lerp(1, 0, elapsedTime / timing);
            PostProcessing.profile.userLut.settings = set;
            elapsedTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        set.contribution = 0;
        PostProcessing.profile.userLut.settings = set;
    }
}