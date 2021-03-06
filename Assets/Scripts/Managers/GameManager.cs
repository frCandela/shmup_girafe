﻿using System;
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

    [Header("Initialisation:")]
    public MouseController PlayerController;
    public CameraController MainCameraController;
    public UIBar MainBar;
    public TextPopupsGenerator TextPopupsGen;
    public LeaderboardText Leaderboard;
    public SoundManager soundManager;
	public GameObject playerExplosion;
	public GameObject bulletImpact;
	public GameObject hackFillerPrefab;
	public Vector3 barPosition;

    [Header("Levels:")]
    public string[] trackNames;
    private PlayableDirector director;
    private PlayableTrack[] tracks;


    [Header("Multiplier Effects:")]
	public Light mainLight;
	public Color alertColor;
	public Light[] lights;
    public LightParameter[] lightColors;
    private LightParameter currentColor;
    private float currentSpeed;
    public float[] tunnelSpeeds;
	private bool _isFlashing;

    private PostProcessingBehaviour PostProcessing;

    public int scoreLossHitVirus = 1;
    [Header("Score:")]
    private int hackPerCombo;
    public int scorePeerHack = 50;

	[HideInInspector]public bool _playWrong = true;
	[HideInInspector] public bool _tutoPlaying = false;
    
    private int score = 0;
    private int[] scores;
    private int hackCount = 0;
    
	private const int maxCombo = 4;	//Jonas : x0 (virus), x1, x2, x4, x8. (was set to 3).
    private int comboMultiplier = 0;
    private float timerCheckpoint;
    private float checkpointRefreshTime;
    private const int checkpointCount = 12;
    private int checkpointId = 0;
   	private float levelDuration = 200;
	private string leaderInit;
    //private float timeLevel = 0;

    //Sound
    FMODUnity.StudioEventEmitter music;

    [Header("THIS IS TEMPORARY:")]
    public int initComboMultiplier = 0;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Singleton pattern
        if (instance == null)
        {
            instance = this;
            //AwakeGame();
        } 
        else if (instance != this)
            Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
    }

	void OnEnable()
	{
		AwakeGame ();
	}

    //Initializes the game for each level.
    void AwakeGame() {
        if (!PlayerController)
            throw new Exception("Error : no player controller selected");
        if (!MainCameraController)
            throw new Exception("Error : no main camera selected");
        if (!TextPopupsGen)
            throw new Exception("Error : no TextPopupsGenerator selected");

        levelDuration = GetComponent<TimeManager>()._gameDuration;

        PostProcessing = MainCameraController.gameObject.GetComponent<PostProcessingBehaviour>();

        PlayerController.PossessVirus();
        PlayerController.onHack.AddListener(hackOccured);
        PlayerController.onHackStart.AddListener(hackStarted);
        PlayerController.onHackStop.AddListener(hackStopped);
        PlayerController.onBecomeVirus.AddListener(playerBecameVirus);
        PlayerController.onTakeDamage.AddListener(playerHit);
        PlayerController.PossessedPawn.transform.position = getMouseWorldPosition();

        //Init variables
        score = 0;
        scores = new int[12];
        hackCount = 0;
        hackPerCombo = 0;
        comboMultiplier = initComboMultiplier;

        //initialise ui
        MainBar.mouseController = (MouseController)PlayerController;
        MainBar.setMulti(comboMultiplier, hackCount);

        //Post Processing reset
        UserLutModel.Settings set = PostProcessing.profile.userLut.settings;
        set.contribution = 0;
        PostProcessing.profile.userLut.settings = set;
		ChromaticAberrationModel.Settings chromaAb = PostProcessing.profile.chromaticAberration.settings;
		chromaAb.intensity = 0;
		PostProcessing.profile.chromaticAberration.settings = chromaAb;
		BloomModel.Settings bloom = PostProcessing.profile.bloom.settings;
		bloom.bloom.intensity = 0;
		PostProcessing.profile.bloom.settings = bloom;

        director = GetComponent<PlayableDirector>();
        tracks = new PlayableTrack[trackNames.Length];
        foreach (TrackAsset track in ((TimelineAsset)director.playableAsset).GetRootTracks()) {
            for(int i = 0; i < trackNames.Length; i++) {
                if(trackNames[i] == track.name) {
                    tracks[i] = (PlayableTrack)track;
                    break;
                }
            }
        }

        //Music
        music = MainCameraController.GetComponent<FMODUnity.StudioEventEmitter>();

        PlayTrack(0);
        director.Play();

        foreach (Light light in lights) {
            light.color = lightColors[0].color;
            light.intensity = lightColors[0].intensity;
        }
        SetLights(0);

        checkpointRefreshTime = levelDuration / checkpointCount;

        timerCheckpoint = checkpointRefreshTime;
        checkpointId = 0;

		leaderInit = Leaderboard.InitLeaderboard ();
    }
    
    private void Update()
    {
		//Press escape to go to menu
		if (Input.GetKeyDown (KeyCode.Escape))
			BackToMenu ();
		
       // timeLevel += Time.deltaTime;

        foreach (Light light in lights) {
            light.color = Color.Lerp(light.color, currentColor.color, Time.deltaTime);
            light.intensity = Mathf.Lerp(light.intensity, currentColor.intensity, Time.deltaTime);
        }

        if(!_tutoPlaying) timerCheckpoint -= Time.deltaTime;
        if(timerCheckpoint < 0)
        {
            timerCheckpoint = checkpointRefreshTime;
            if (checkpointId < checkpointCount - 1)
                saveScore(checkpointId);
			Leaderboard.UpdateScore(checkpointId);
			checkpointId++;
            if (checkpointId >= checkpointCount)
                checkpointId = checkpointCount - 1;
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
            TextPopupsGen.generateScorePopup(-scoreLossHitVirus, PlayerController.PossessedPawn.transform.position);
        }
       
    }

    private void hackStarted()
    {
        music.SetParameter("hack", 1);
        FMODUnity.RuntimeManager.PlayOneShot("event:/hack/hack_début",  MainCameraController.transform.position );
    }
    private void hackStopped()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/wrong", MainCameraController.transform.position);
        music.SetParameter("hack", 0);
    }

    private void hackOccured()
    {
        //Increment combo multiplier
        ++hackCount;
        if (hackCount > hackPerCombo && comboMultiplier < maxCombo)
        {
            hackCount = 0;
           	++comboMultiplier;

            if (comboMultiplier == 1)
                hackPerCombo = 0;
            else
                hackPerCombo = comboMultiplier-1;
            PlayTrack(comboMultiplier);
            SetLights(comboMultiplier);

			//feedback light effect
			StartCoroutine (ComboLightEffect ());

            //Music
            FMODUnity.RuntimeManager.PlayOneShot("event:/hack/hack_fin", MainCameraController.transform.position);
            FMODUnity.RuntimeManager.PlayOneShot("event:/mult", MainCameraController.transform.position);

            music.SetParameter("combo", comboMultiplier+0.1f);
        }
        MainBar.setMulti(comboMultiplier, hackCount);

        music.SetParameter("hack", 0);

        int scoreGained = addScore(scorePeerHack);
        TextPopupsGen.generateScorePopup(scoreGained, PlayerController.PossessedPawn.transform.position);
    }

    void PlayTrack(int level) {
        for (int i = 0; i < tracks.Length; i++) {
            tracks[i].muted = (i != level);
        }
        director.initialTime = director.time;
        director.Stop();
        director.Play();
    }

	public void DecreaseMulti()
	{
		hackCount = 0;
		--comboMultiplier;
		if (comboMultiplier == 1)
			hackPerCombo = 0;
		else
			hackPerCombo = comboMultiplier - 1;
		PlayTrack (comboMultiplier);
		SetLights (comboMultiplier);

		//feedback light effect
		StartCoroutine (ComboFlash ());

		music.SetParameter ("combo", comboMultiplier + 0.1f);

		MainBar.setMulti (comboMultiplier, hackCount);

		music.SetParameter ("hack", 0);

		FMODUnity.RuntimeManager.PlayOneShot("event:/explosion_player", transform.position);
	}

	public void playerBecameVirus()
    {
		//feedback light effect
		if(!_isFlashing)
		{
			StartCoroutine (ComboFlash ());

			//Reset the combo
			hackCount = 0;
			hackPerCombo = 0;
			comboMultiplier = 0;

			//initialise ui
			MainBar.setMulti (comboMultiplier, hackCount);

			//Music
			music.SetParameter ("combo", 0);
			//FMODUnity.RuntimeManager.PlayOneShot("event:/demult", MainCameraController.transform.position);

			PlayTrack (0);
			SetLights (0);
		}
    }

    public int getScore(){ return score; }

    public int getMulti()
    {
        if (comboMultiplier == 0)
            return 0;
        else
            return (int)Mathf.Pow(2f, comboMultiplier - 1);
    }

    //Returns the score effectively gained by the player
    public int addScore(int rawScore)
    {
        int scoreGained = getMulti() * rawScore;
        score += scoreGained;
        return scoreGained;
    }

	public void ResetGameState()
	{
		score = 0;
		//Reset the combo
		hackCount = 0;
		hackPerCombo = 0;
		comboMultiplier = 0;

		//initialise ui
		MainBar.setMulti(comboMultiplier, hackCount);

		//Music
		music.SetParameter("combo", 0);
		//FMODUnity.RuntimeManager.PlayOneShot("event:/demult", MainCameraController.transform.position);

		PlayTrack(0);
		SetLights(0);
		timerCheckpoint = checkpointRefreshTime;
		checkpointId = 0;
		Leaderboard.ResetLeaderboard (leaderInit);
		PlayerController.addHackPower (-100f);
		_tutoPlaying = false;
	}

    public void saveScore(int check) { scores[check] = score; }
    public int[] getScores() { return scores; }

    void SetLights(int mult) {
        currentColor = lightColors[mult];
    }

    public float getTunnelSpeed() {
        currentSpeed = Mathf.Lerp(currentSpeed, tunnelSpeeds[comboMultiplier], Time.deltaTime);
        return currentSpeed;
    }

	//Light effect when multiplier changes
	IEnumerator ComboLightEffect()
	{
		float elapsedTime = 0f;
		float timing = 0.5f;
		float intensity = mainLight.intensity;
		Quaternion rotation = mainLight.transform.rotation;
		while(elapsedTime < timing)
		{
			if (mainLight.intensity < 40f)
				mainLight.intensity += 20f*Time.deltaTime;
			mainLight.transform.Rotate (0, 2160f*Time.deltaTime, 0);
			elapsedTime += Time.unscaledDeltaTime;
			yield return new WaitForEndOfFrame ();
		}

		mainLight.transform.rotation = rotation;

		while (mainLight.intensity > intensity)
		{
			mainLight.intensity -= 20f*Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		mainLight.intensity = intensity;
	}

	IEnumerator ComboFlash()
	{
		_isFlashing = true;
		Color originalColor = mainLight.color;
		mainLight.color = alertColor;

		float elapsedTime = 0f;
		float timing = 0.5f;
		float intensity = mainLight.intensity;

		while(elapsedTime < timing)
		{
			if (mainLight.intensity < 5f)
				mainLight.intensity += 10f*Time.deltaTime;
			elapsedTime += Time.unscaledDeltaTime;
			yield return new WaitForEndOfFrame ();
		}
	
		while (mainLight.intensity > 0)
		{
			mainLight.intensity -= 10f*Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		mainLight.color = originalColor;
		while (mainLight.intensity < intensity)
		{
			mainLight.intensity += 10f*Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		_isFlashing = false;
	}

	public void BackToMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Title");
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
		ChromaticAberrationModel.Settings chromaAb = PostProcessing.profile.chromaticAberration.settings; //Jonas
		BloomModel.Settings bloom = PostProcessing.profile.bloom.settings;

        float elapsedTime = 0;
        while (elapsedTime < timing)
        {
			//color
            set.contribution = Mathf.Lerp(0, 1, elapsedTime / timing);
            PostProcessing.profile.userLut.settings = set;

			//chroma aberration
			chromaAb.intensity = Mathf.Lerp(0.114f, 1, elapsedTime / timing);
			PostProcessing.profile.chromaticAberration.settings = chromaAb;

			//bloom
			bloom.bloom.intensity =Mathf.Lerp(0, 0.25f, elapsedTime / timing);
			PostProcessing.profile.bloom.settings = bloom;

            elapsedTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        set.contribution = 1;
        PostProcessing.profile.userLut.settings = set;

		chromaAb.intensity = 1;
		PostProcessing.profile.chromaticAberration.settings = chromaAb;

		bloom.bloom.intensity = 0.25f;
		PostProcessing.profile.bloom.settings = bloom;
    }

    IEnumerator stopHack(float timing)
    {
        UserLutModel.Settings set = PostProcessing.profile.userLut.settings;
		ChromaticAberrationModel.Settings chromaAb = PostProcessing.profile.chromaticAberration.settings; //Jonas
		BloomModel.Settings bloom = PostProcessing.profile.bloom.settings;

        float elapsedTime = 0;
        while (elapsedTime < timing)
        {
			//color
            set.contribution = Mathf.Lerp(1, 0, elapsedTime / timing);
            PostProcessing.profile.userLut.settings = set;

			//chroma aberration
			chromaAb.intensity = Mathf.Lerp(1, 0.114f, elapsedTime / timing);
			PostProcessing.profile.chromaticAberration.settings = chromaAb;

			//bloom
			bloom.bloom.intensity =Mathf.Lerp(0.25f, 0, elapsedTime / timing);
			PostProcessing.profile.bloom.settings = bloom;

            elapsedTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        set.contribution = 0;
        PostProcessing.profile.userLut.settings = set;

		chromaAb.intensity = 0.114f;
		PostProcessing.profile.chromaticAberration.settings = chromaAb;

		bloom.bloom.intensity = 0;
		PostProcessing.profile.bloom.settings = bloom;
    }
    #endregion
}