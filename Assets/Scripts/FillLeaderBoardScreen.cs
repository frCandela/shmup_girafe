using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillLeaderBoardScreen : MonoBehaviour 
{
	[SerializeField] private Text pseudos, scores;
	private List<Record> leaders;

	void OnEnable()
	{
		leaders = new List<Record> ();
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
}
