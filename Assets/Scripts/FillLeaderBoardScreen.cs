using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillLeaderBoardScreen : MonoBehaviour 
{
	[SerializeField] private Text pseudos, scores;
	private List<Record> leaders;
	private Record playerScore;

	void OnEnable()
	{
		leaders = new List<Record> ();
		StartCoroutine(OnlineScore.GetScores(leaders, 11));
		playerScore.name = GameManager.instance.MainBar.input.text;
		playerScore.score = GameManager.instance.getScore ();
	}

	void Update()
	{
		if (leaders.Count > 0)
		{
			pseudos.text = "";
			scores.text = "";
			foreach (Record r in leaders)
			{
				if(r.name.ToLower () == playerScore.name.ToLower () && r.score == playerScore.score)
				{
					pseudos.text += "<color=red>" + r.name + "</color>" + "\n";
					scores.text += "<color=red>" + r.score + "</color>" + "\n";
				}
				else
				{
					pseudos.text += r.name + "\n";
					scores.text += r.score + "\n";
				}
			}
		}
		else scores.text = "No internet connection... Your score will not be sAved.";
	}
}
