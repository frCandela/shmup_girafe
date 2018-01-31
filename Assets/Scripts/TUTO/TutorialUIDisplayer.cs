using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIDisplayer : MonoBehaviour
{
	[SerializeField] private GameObject[] _steps;

	void Start()
	{
		foreach (GameObject step in _steps)
			if(step )step.SetActive (false);
	}

	public IEnumerator DisplayStep(int index, bool active)
	{
		_steps [index].SetActive (active);
		yield return null;
		/*if (active) 
			yield return StartCoroutine (_steps [index].GetComponent<TutoStepDisplay> ().Display (true));
		else
			yield return StartCoroutine (_steps [index].GetComponent<TutoStepDisplay> ().Display (false));
			*/
	}

	/*public void DisplayStep(int index, bool active)
	{
		if (active) {
			_steps [index].SetActive (true);
		} else
			StartCoroutine (_steps [index].GetComponent<TutoStepDisplay> ().Display (false));
	}*/

	public void ToggleInfo (int index, bool active)
	{
		_steps [index].SetActive (active);
	}
}
