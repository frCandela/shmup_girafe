using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitcher : MonoBehaviour
{
	[SerializeField] private float _glitchDuration = 0.3f;
	private GlitchEffect _glitch;

	void Awake()
	{
		_glitch = GetComponent<GlitchEffect> ();
	}

	public void DoGlitchEffect()
	{
		StartCoroutine (GlitchEffect());
	}

	public IEnumerator GlitchEffect()
	{
		float elapsedTime = 0f;
		//START GLITCH EFFECT
		_glitch.enabled = true;
		_glitch.flipIntensity = 1f;
		_glitch.colorIntensity = 1f;

		while(elapsedTime < _glitchDuration)
		{
			elapsedTime += Time.unscaledDeltaTime;
			yield return null;
		}

		//END GLITCH EFFECT
		_glitch.flipIntensity = 0f;
		_glitch.colorIntensity = 0f;
		_glitch.enabled = false;
	}
}
