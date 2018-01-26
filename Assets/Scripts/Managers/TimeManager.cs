using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TimeManager (Singleton pattern)
public class TimeManager : MonoBehaviour
{
	//Timer variables
	[Tooltip("In seconds")]
	[SerializeField] private float _gameDuration = 180f;
	[SerializeField] private Text _timerText;
	public bool _countdown = true;
	[SerializeField] private float _countdownDuration = 3f;
	[SerializeField] private Text _countdownText;
	private float _elapsedTime = 0f;
	private float _mins;
	private float _secs;
	private float _cents;
	private float _timeLeft;
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
        DontDestroyOnLoad(gameObject);
        InitTime();
		if(!_countdown)
		{
			_gameStarted = true;
		}
    }

    private void Update()
    {
        m_timeElapsedSlowMo += Time.unscaledDeltaTime;

        if (m_slowDownDuration != 0F && m_timeElapsedSlowMo >= m_slowDownDuration)
            resetSlowMotion();
        
        if(!_gameStarted)
		{
			DisplayCountdown ();
			_elapsedTime += Time.unscaledDeltaTime;
		}
		else 
		{
			DisplayTimer ();
			_elapsedTime += Time.unscaledDeltaTime;
		}
    }

	void DisplayCountdown()
	{
		if (_countdownText) {
			_timeLeft = _countdownDuration - Mathf.Floor (_elapsedTime);
			_countdownText.text = _timeLeft.ToString ("F0");
			if (_timeLeft < _countdownDuration) {
				_elapsedTime = 0f;
				_gameStarted = true;
			}
		} else {
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
        }
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
