using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{

    public int score;

	public void AddScore()
    {
        int effectiveScore = GameManager.instance.addScore(score);
        GameManager.instance.TextPopupsGen.generateScorePopup(effectiveScore, transform.position);
    }
}
