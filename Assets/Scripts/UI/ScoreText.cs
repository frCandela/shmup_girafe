using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour
{
    private Text text;
	private int currentScore = 0;

    void Start()
    {
        text = GetComponent<Text>();
    }

    public void Update()
    {
		if (text) {
			int newScore = GameManager.instance.getScore (); 
			if (newScore > currentScore) {
				currentScore = (int)Mathf.Lerp (currentScore, newScore, 10f * Time.deltaTime);
				if (Mathf.Abs (newScore - currentScore) < 5)
					currentScore = newScore;
			} else if (newScore < currentScore) {
				{
					currentScore = (int)Mathf.Lerp (newScore, currentScore, 10f * Time.deltaTime);
					if (Mathf.Abs (newScore - currentScore) < 5)
						currentScore = newScore;
				}
			}
			text.text = string.Format ("{0:D10}", currentScore);
		}
    }
}
