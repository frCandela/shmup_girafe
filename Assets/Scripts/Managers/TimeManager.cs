using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TimeManager (Singleton pattern)
public class TimeManager : MonoBehaviour
{
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
    }

    private void Update()
    {
        m_timeElapsedSlowMo += Time.unscaledDeltaTime;

        if (m_slowDownDuration != 0F && m_timeElapsedSlowMo >= m_slowDownDuration)
            resetSlowMotion();
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
