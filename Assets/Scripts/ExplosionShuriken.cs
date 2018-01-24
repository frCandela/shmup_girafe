using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionShuriken : MonoBehaviour 
{
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive ()
	{
		Animation anim = this.GetComponent<Animation>();

		while(true && anim != null)
		{
			yield return new WaitForSeconds(0.5f);
			if(!anim.isPlaying)
			{
				GameObject.Destroy(this.gameObject);
			}
		}
	}

	void Destroy()
	{
		//GetComponent<SpriteRenderer> ().sprite = null;
		Destroy (gameObject);
	}
}
