using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIDisplayer : MonoBehaviour
{
	[SerializeField] private GameObject[] _steps;

	void Start()
	{
		//_steps [3] = GameManager.instance.PlayerController.PossessedPawn.gameObject.transform.Find ("Canvas").gameObject;
		foreach (GameObject step in _steps)
			if(step )step.SetActive (false);
	}

	public IEnumerator DisplayStep(int index, bool active)
	{
		if(_steps[index].GetComponent<Image>())
		{
			Image im = _steps [index].GetComponent<Image> ();
			float elapsedTime = 0f;
			float timing = 0.1f;
			float fadeOut;

			if(active)
			{
				while (elapsedTime < timing) 
				{
					fadeOut = Mathf.Lerp (0f, 1f, elapsedTime / timing);
					im.color = new Color (1f, 1f, 1f, fadeOut);
					elapsedTime += Time.unscaledDeltaTime;
					yield return new WaitForEndOfFrame ();
				}
				im.color = new Color (1f, 1f, 1f, 1f);
			}
			else
			{
				while (elapsedTime < timing) 
				{
					fadeOut = Mathf.Lerp (1f, 0f, elapsedTime / timing);
					im.color = new Color (1f, 1f, 1f, fadeOut);
					elapsedTime += Time.unscaledDeltaTime;
					yield return new WaitForEndOfFrame ();
				}
				im.color = new Color (1f, 1f, 1f, 0f);
			}
		}
		yield return null;
		_steps [index].SetActive (active);
	}

	public void ToggleInfo (int index, bool active)
	{
		_steps [index].SetActive (active);
	}
}
