using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LeaderboardText : MonoBehaviour {
	[SerializeField] private int _scoresDisplayed = 3;
	private Text text;
    private List<Record> leaders;

    void Awake() {
        text = GetComponent<Text>();
        leaders = new List<Record>();
    }

    void Update() {
        if(leaders.Count > 0) {
            text.text = "<color=grey>LeAderboArd:</color>\n";
            int count = 1;
            int playerScore = GameManager.instance.getScore();
            bool showPlayer = false;
			for (int i = 0 ; i < _scoresDisplayed; i++)
			{
				Record r = leaders [i];
                if(playerScore > r.score && !showPlayer) {
                    text.text += "<color=red>" + (count++) + ". YOU " + playerScore + "</color>\n";
                    showPlayer = true;
                }

                text.text += "" + (count++) + ". " + r.name + " " + r.score + "\n";
            }
            if(!showPlayer) {
                text.text += "<color=red>" + (count++) + ". YOU " + playerScore + "</color>\n";
            }
        }
    }

    public void UpdateScore(int i) {
        StartCoroutine(OnlineScore.GetScores(leaders, i));
    }
}

public struct Record {
    public string name;
    public int score;
}