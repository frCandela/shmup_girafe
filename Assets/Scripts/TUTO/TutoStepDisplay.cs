using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoStepDisplay : MonoBehaviour 
{
	/*[SerializeField] private RectTransform _window;
	private float xScale;
	private float yScale;
	private RectTransform rect;

	public IEnumerator Display(bool active)
	{
		
		float elapsedTime = 0f;
		float timing = 0.1f;
		float new_yScale;

		if(active)
		{
			gameObject.SetActive (true);
			yield return new WaitForSeconds (0.1f);
			while (elapsedTime < timing) 
			{
				new_yScale = Mathf.Lerp (0f, yScale, elapsedTime / timing);
				_window.localScale = new Vector3 (xScale, new_yScale, 1f);
				elapsedTime += Time.unscaledDeltaTime;
				yield return new WaitForEndOfFrame ();
			}
			_window.localScale = new Vector3 (xScale, yScale, 1f);
		}
		else
		{
			while (elapsedTime < timing) 
			{
				new_yScale = Mathf.Lerp (yScale, 0f, elapsedTime / timing);
				_window.localScale = new Vector3 (xScale, new_yScale, 1f);
				elapsedTime += Time.unscaledDeltaTime;
				yield return new WaitForEndOfFrame ();
			}
			_window.localScale = new Vector3 (xScale, yScale, 1f);
			gameObject.SetActive (false);
		}
	}*/
}
