using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TimeManager (Singleton pattern)
public class TimeManager : MonoBehaviour
{
	//Timer variables
	[Tooltip("In seconds")]
	public float _gameDuration = 180f;
	[SerializeField] private Text _timerText;
	[SerializeField] private bool _countdown = true;
	[SerializeField] private float _countdownDuration = 5f;
	[SerializeField] private GameObject _countdownDisplay;
	[SerializeField] private Sprite[] _countdownImages;
	[SerializeField] private Image _fadeOutImage;
	private Image _countdownImage;
	private float _elapsedTime = 0f;
	private float _elapsedCD = 0f;
	private float _mins;
	private float _secs;
	private float _cents;
	private float _timeLeft;
	private float _countdownLeft;
	private bool _gameStarted = false;

    private static float m_slowDownDuration;
    private static float m_timeElapsedSlowMo;

    private static bool m_inSlowMo = false;

    public static TimeManager instance = null;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Singleton pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
        InitTime();
    }

	void Start()
	{
		_countdownImage = _countdownDisplay.GetComponent<Image> ();
		_timerText.color = Color.white;
		if (!_countdown)
			_gameStarted = true;
		else {
			_countdownDisplay.SetActive (true);
			StartCoroutine (DisplayCountdown ());
		}
	}

    private void Update()
    {
        m_timeElapsedSlowMo += Time.unscaledDeltaTime;

        if (m_slowDownDuration != 0F && m_timeElapsedSlowMo >= m_slowDownDuration)
            resetSlowMotion();
        
		DisplayTimer ();
        if(_gameStarted)
			_elapsedTime += Time.unscaledDeltaTime;
    }

	IEnumerator DisplayCountdown()
	{
		if (_countdownDisplay) 
		{
			while(_elapsedCD < _countdownDuration)
			{
				_countdownLeft = _countdownDuration - Mathf.Floor (_elapsedCD);
				_countdownImage.sprite = _countdownImages [(int)_countdownLeft];
				yield return null;
				_elapsedCD += Time.unscaledDeltaTime;
			}
			_countdownImage.sprite = _countdownImages [0];
			_elapsedCD = 0f;
			_gameStarted = true;
			yield return new WaitForSeconds (1f);
			_countdownDisplay.SetActive (false);

		} else 
		{
			_gameStarted = true;
			Debug.LogWarning("No countdown text assigned, game will start automatically.");
		}
	}

	void DisplayTimer()
	{
        if(_timerText)
        {
            _timeLeft = _gameDuration - _elapsedTime;
            _mins = Mathf.Floor(_timeLeft / 60);
            _secs = Mathf.Floor(_timeLeft % 60);
            _cents = Mathf.Round(_timeLeft * 100) % 100;
            _timerText.text = string.Format("{0:0}:{1:00}:{2:00}", _mins, _secs, _cents);

			//red last ten seconds
			if (_timeLeft < 10f) 
			{
				_timerText.color = Color.red;
				if(_timeLeft < 0f)
				{
					_timeLeft = 0f;
					_timerText.text = string.Format("{0:0}:{1:00}:{2:00}", 0,00,00);
					if(_gameStarted) StartCoroutine (EndAnimation ());
					_gameStarted = false;
				}
			}
        }
	}

	IEnumerator EndAnimation()
	{
		GameManager.instance.scoreLossHitVirus = 0;
		float timing = 3f;
		float elapsedTime = 0f;
		float speed = 0f;
		float fadeOut = 0f;
		while(elapsedTime < timing)
		{
			//speed up camera
			speed = Mathf.Lerp (0, 100f, elapsedTime / timing);
			GameManager.instance.MainCameraController.VerticalSpeed = speed;

			//fade out
			fadeOut = Mathf.Lerp (0, 1f, elapsedTime / timing);
			_fadeOutImage.color = new Color (0f, 0f, 0f, fadeOut);

			elapsedTime += Time.unscaledDeltaTime;
			yield return new WaitForEndOfFrame ();
		}
		GameManager.instance.MainCameraController.VerticalSpeed = 100f;
		_fadeOutImage.color = Color.black;
		//pop up leaderboard
		GameManager.instance.MainBar.showLeaderScreen ();
	}

    //Resets the slow motion
    public static void resetSlowMotion()
    {
        Time.timeScale = 1F;
        Time.fixedDeltaTime = Time.fixedUnscaledDeltaTime;
        m_slowDownDuration = 0F;
        m_inSlowMo = false;
        if (GameManager.instance)
            GameManager.instance.setHackEffect(false);
    }

    public static void doSlowMotion(float slowDownDuration, float timeScaleFactor)
    {
        Time.timeScale = timeScaleFactor;
        m_slowDownDuration = slowDownDuration;
        m_timeElapsedSlowMo = 0F;
        m_inSlowMo = true;
        Time.fixedDeltaTime = Time.fixedUnscaledDeltaTime * timeScaleFactor;
        if (GameManager.instance)
            GameManager.instance.setHackEffect(true);
    }

    public static bool inSlowMotion() {
        return m_inSlowMo;
    }

    void InitTime()
    {
        resetSlowMotion();
    }

}
