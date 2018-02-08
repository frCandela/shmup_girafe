using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour
{
	[SerializeField] private int digitsToDisplay = 10;
	[SerializeField] private float maxFontSize = 90f;
    private Text text;
	private int currentScore = 0;
	private string zeros = "";
	private string color = "";
	private string size = "";
	private float minFontSize = 70f;
	private float fontSize;

    void Start()
    {
        text = GetComponent<Text>();
		minFontSize = text.fontSize;
    }

    public void Update()
    {
		if (text)
		{
			int newScore = GameManager.instance.getScore (); 

			if (newScore > currentScore)
			{
				if(fontSize < maxFontSize)fontSize = Mathf.Lerp (minFontSize, maxFontSize, 30f * Time.deltaTime);
				size = "<Size=" +fontSize.ToString () + ">";
				color = "<Color=green>";
				currentScore = (int)Mathf.Lerp (currentScore, newScore, 10f * Time.deltaTime);
				if (Mathf.Abs (newScore - currentScore) < 5)
					currentScore = newScore;
			} 
			else if (newScore < currentScore) 
			{
				if(fontSize < maxFontSize)fontSize = Mathf.Lerp (minFontSize, maxFontSize, 30f * Time.deltaTime);
				size = "<Size=" +fontSize.ToString () + ">";
				color = "<Color=red>";
				currentScore = (int)Mathf.Lerp (newScore, currentScore, 10f * Time.deltaTime);
				if (Mathf.Abs (newScore - currentScore) < 5)
					currentScore = newScore;
			} else
			{
				size = "<Size=" + minFontSize.ToString () + ">";
				color = "<Color=white>";
			}
			zeros = "";
			int zerosToDisplay = digitsToDisplay - currentScore.ToString ().Length;
			for (int i = 0 ; i < zerosToDisplay ; i++)
			{
				zeros += "0";
			}
			text.text = string.Format (zeros + size + color +currentScore.ToString ()+ "</color></size>") ;				
		}
    }
}
