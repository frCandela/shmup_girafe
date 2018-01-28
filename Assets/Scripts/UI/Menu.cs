using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public FMODUnity.StudioEventEmitter music;

    public GameObject credits, loading, leader, tutoriel, background;
    public Text pseudos, scores;

    private List<Record> leaders;

    public GameObject[] slide;
    public GameObject button;
    private int slideId = 0;

    void Awake()
    {
        leaders = new List<Record>();
        StartCoroutine(OnlineScore.GetScores(leaders, 11));
    }

    void Update()
    {
        if (leaders.Count > 0)
        {
            pseudos.text = "";
            scores.text = "";
            foreach (Record r in leaders)
            {
                pseudos.text += r.name + "\n";
                scores.text += r.score + "\n";
            }
        }
    }

    public void ShowCredits()
    {
        credits.SetActive(true);
    }

    public void HideCredits()
    {
        credits.SetActive(false);
    }

    public void ShowLeader()
    {
        leader.SetActive(true);
        StartCoroutine(OnlineScore.GetScores(leaders, 11));
    }

    public void HideLeader()
    {
        leader.SetActive(false);
    }

    public void PlayGame()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("Tutorial");
		tutoriel.SetActive (false);
		background.SetActive (false);
        loading.SetActive(true);
        async.completed += OnLoad;
    }

    public void Tutoriel()
    {
        tutoriel.SetActive(true);
    }

    public void NextSlide()
    {
        slideId++;
        if (slideId == slide.Length - 1)
            button.SetActive(false);
        if (slideId == slide.Length)
            PlayGame();
        else
        {
            slide[slideId - 1].SetActive(false);
            slide[slideId].SetActive(true);
        }
    }

    private void OnLoad(AsyncOperation obj)
    {
        music.Stop();
    }

    public void QuitGame()
    {
        Debug.LogWarning("Quit game");
        Application.Quit();
    }
    
}