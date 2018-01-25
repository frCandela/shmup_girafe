using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    [FMODUnity.EventRef]
    public string PlayerIntroEvent;
    public FMODUnity.StudioEventEmitter music;

    public GameObject credits, loading, leader;
    public Text pseudos, scores;

    private List<Record> leaders;

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
        loading.SetActive(true);
        async.completed += OnLoad;
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