using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TimeManager (Singleton pattern)
public class TimeManager : MonoBehaviour
{
    private float timeScaleFactor;
    private float slowDownDuration;

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

    public static void doSlowMotion(float slowDownDuration, float timeScaleFactor)
    {
        Time.timeScale = timeScaleFactor; 
    }

    void InitTime()
    {

    }

}
