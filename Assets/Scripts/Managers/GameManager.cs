using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public struct LightParameter {
    public float intensity;
    public Color color;
}

//GameManager (Singleton pattern)
public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    /*public static Ship StarterShip { get; private set; }
    public static MouseController PlayerController { get; private set; }
    public static CameraController MainCameraController { get; private set; }
    public static MainBar MainBar { get; private set; }*/

    [Header("Initialisation:")]
    public MouseController PlayerController;
    public CameraController MainCameraController;
    public MainBar MainBar;
    public TextPopupsGenerator TextPopupsGen;
    public LeaderboardText Leaderboard;


    [Header("Levels:")]
    public PlayableDirector director;
    public TimelineAsset[] timelines;


    [Header("Multiplier Effects:")]
    public Light[] lights;
    public LightParameter[] lightColors;
    public float[] tunnelSpeeds;

    private PostProcessingBehaviour PostProcessing;


    public const int scoreLossHitVirus = 1;
    [Header("Score:")]
    public int hackPerCombo = 2;
    public int scorePeerHack = 50;

    private int score = 0;
    private int[] scores = new int[5];
    private int hackCount = 0;
    private int comboMultiplier = 0;
	private const int maxCombo = 4;	//Jonas : x0 (virus), x1, x2, x4, x8. (was set to 3).

    private float timerCheckpoint;
    public float checkpointRefreshTime = 5;
    private const int checkpointCount = 5;
    private int checkpointId = 0;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Singleton pattern
        if (instance == null)
        {
            instance = this;
            AwakeGame();
        } 
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    //Initializes the game for each level.
    void AwakeGame()
    {
        if (!PlayerController)
            throw new Exception("Error : no player controller selected");
        if (!MainCameraController)
            throw new Exception("Error : no main camera selected");
        if (!TextPopupsGen)
            throw new Exception("Error : no TextPopupsGenerator selected");
        
        PostProcessing = MainCameraController.gameObject.GetComponent<PostProcessingBehaviour>();

        PlayerController.PossessVirus();
        PlayerController.addHackPower(50);
        PlayerController.onHack.AddListener(hackOccured);
        PlayerController.onBecomeVirus.AddListener(playerBecameVirus);
        PlayerController.onTakeDamage.AddListener(playerHit);
        //PlayerController.PossessedPawn.transform.position = getMouseWorldPosition();

        //Init variables
        score = 0;
        scores = new int[5];
        hackCount = 0;
        hackPerCombo = 0; //Jonas : set to 0, was to 1.
        comboMultiplier = 3;

        //initialise ui
        MainBar.mouseController = (MouseController)PlayerController;
        MainBar.setCombo(0);
        //MainBar.setMulti(getMulti());
		//Jonas
		MainBar.setMulti (0);//
        MainBar.setSegments(hackPerCombo);

        //Post Processing reset
        UserLutModel.Settings set = PostProcessing.profile.userLut.settings;
        set.contribution = 0;
        PostProcessing.profile.userLut.settings = set;

        director.playableAsset = timelines[0];
        director.Play();

        SetLights(0);

        timerCheckpoint = checkpointRefreshTime;
        checkpointId = 0;
        Leaderboard.UpdateScore(checkpointId);
    }

    private void Update()
    {

        timerCheckpoint -= Time.deltaTime;
        if(timerCheckpoint < 0)
        {
            timerCheckpoint = checkpointRefreshTime;
            checkpointId++;
            if (checkpointId >= checkpointCount)
                checkpointId = checkpointCount - 1;
            Leaderboard.UpdateScore(checkpointId);
        }
    }

    //Return the position of the mouse in world coordinates
    public Vector3 getMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    //When the player is hit by a bullet
    private void playerHit()
    {
        if (PlayerController.isVirus())
        {
            score -= scoreLossHitVirus;
            if (score < 0)
                score = 0;

            TextPopupsGen.generateScorePopup( - scoreLossHitVirus, PlayerController.PossessedPawn.transform.position); 
        }
    }

    private void hackOccured()
    {
        //Increment combo multiplier
        if ( ++hackCount > hackPerCombo && comboMultiplier < maxCombo)
        {
            hackCount = 0;
           	//++comboMultiplier;
            ++hackPerCombo;

            MainBar.setSegments(comboMultiplier + 1);
            MainBar.setMulti(getMulti());

			++comboMultiplier;	//Jonas

            director.playableAsset = timelines[comboMultiplier];
            director.initialTime = -2;
            director.Play();
            SetLights(comboMultiplier);
        }

        MainBar.setCombo(hackCount);

        int scoreGained = addScore(scorePeerHack);
        TextPopupsGen.generateScorePopup(scoreGained, PlayerController.PossessedPawn.transform.position);
    }

    public void playerBecameVirus()
    {
        //Reset the combo
        hackCount = 0;
        comboMultiplier = 0;


        MainBar.setCombo(hackCount + 1);
        MainBar.setSegments(1); 
        //MainBar.setMulti(getMulti());
		//Jonas
		MainBar.setMulti(0);

        director.playableAsset = timelines[0];
        director.initialTime = -4;
        director.Play();
        SetLights(0);
    }

    public int getScore(){ return score; }

    public int getMulti() { return (int)Mathf.Pow(2f, comboMultiplier);}

    //Returns the score effectively gained by the player
    public int addScore(int rawScore)
    {
        int scoreGained = getMulti() * rawScore;
        score += scoreGained;
        return scoreGained;
    }
    public void saveScore(int check) { scores[check] = score; }

    void SetLights(int mult) {
        foreach(Light light in lights) {
            light.color = lightColors[mult].color;
            light.intensity = lightColors[mult].intensity;
        }
    }

    public float getTunnelSpeed() {
        return tunnelSpeeds[comboMultiplier];
    }

    #region POST-EFFECT

    bool inHackEffect = false;
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