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
    public static MouseController PlayerController { get; private set; }
    public static CameraController MainCameraController { get; private set; }
    public static MainBar MainBar { get; private set; }

    [Header("Initialisation:")]
    public Ship InitStarterShip;
    public MouseController InitPlayerController;
    public CameraController InitMainCameraController;
    public MainBar InitMainBar;

    private PostProcessingBehaviour PostProcessing;

    public const int scorePeerHack = 10;
    public const int hackPerCombo = 5;

    private int score = 0;
    private int hackCount = 0;
    private int comboMultiplier = 0;
    private const int maxCombo = 3;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Singleton pattern
        if (instance == null)
        {
            instance = this;
            InitGame();
        } 
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //Initialisation checks
        if (!InitStarterShip)
            throw new Exception("Error : no starter ship selected");

        if (!InitPlayerController)
            throw new Exception("Error : no player controller selected");

        if (!InitMainCameraController)
            throw new Exception("Error : no main camera selected");

        StarterShip = InitStarterShip;
        PlayerController = InitPlayerController;
        MainCameraController = InitMainCameraController;
        MainBar = InitMainBar;
        PostProcessing = MainCameraController.gameObject.GetComponent<PostProcessingBehaviour>();

        //Initialize player
        PlayerController.Possess(StarterShip);
        PlayerController.onHack.AddListener(hackOccured);
        PlayerController.onBecomeVirus.AddListener(playerBecameVirus);

        //initialise ui
        MainBar.mouseController = (MouseController)PlayerController;
        MainBar.health = StarterShip.GetComponent<Health>();
        MainBar.setCombo(1);

        //Init variables
        score = 0;
        hackCount = 0;
        comboMultiplier = 0;
    }

    public void hackOccured()
    {
        //Add score
        score += scorePeerHack *(int) Mathf.Pow(2, comboMultiplier);

        //Increment combo multiplier
        if ( ++hackCount >= hackPerCombo && comboMultiplier < maxCombo)
        {
            hackCount = 0;
            ++comboMultiplier;
            MainBar.setCombo(comboMultiplier+1);
        }
    }

    public void playerBecameVirus()
    {
        //Reset the combo
        hackCount = 0;
        comboMultiplier = 0;
        MainBar.setCombo(comboMultiplier + 1);
    }

    public int getScore(){ return score;}

    #region POST-EFFECT
    bool inHackEffect = false;
    public void ToogleHackEffect()
    {
        if (!inHackEffect)
        {
            StopCoroutine(stopHack(1f));
            StartCoroutine(startHack(1f));
        }
        else
        {
            StopCoroutine(startHack(1f));
            StartCoroutine(stopHack(1f));
        }
        inHackEffect = !inHackEffect;
    }

    public void setHackEffect( bool state )
    {
        if(inHackEffect != state)
        {
            inHackEffect = state;
            if (state)
            {
                StopCoroutine(stopHack(1f));
                StartCoroutine(startHack(1f));
            }
            else
            {
                StopCoroutine(startHack(1f));
                StartCoroutine(stopHack(1f));
            }
        }
    }
    
    IEnumerator startHack(float timing)
    {
        UserLutModel.Settings set = PostProcessing.profile.userLut.settings;

        float elapsedTime = 0;
        while (elapsedTime < timing)
        {
            set.contribution = Mathf.Lerp(0, 1, elapsedTime / timing);
            PostProcessing.profile.userLut.settings = set;
            elapsedTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        set.contribution = 1;
        PostProcessing.profile.userLut.settings = set;
    }

    IEnumerator stopHack(float timing)
    {
        UserLutModel.Settings set = PostProcessing.profile.userLut.settings;

        float elapsedTime = 0;
        while (elapsedTime < timing)
        {
            set.contribution = Mathf.Lerp(1, 0, elapsedTime / timing);
            PostProcessing.profile.userLut.settings = set;
            elapsedTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        set.contribution = 0;
        PostProcessing.profile.userLut.settings = set;
    }
    #endregion
}