using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

	public void AddScore(int point)
    {
         int effectiveScore = GameManager.instance.addScore(point);
        GameManager.instance.TextPopupsGen.generateScorePopup(effectiveScore.ToString(), transform.position);
    }
}
