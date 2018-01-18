using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LeaderboardText : MonoBehaviour {
    private Text text;

    void Awake() {
        text = GetComponent<Text>();
    }

    public void UpdateScore(int i) {
        StartCoroutine(OnlineScore.GetScores(text, i));
    }
}
