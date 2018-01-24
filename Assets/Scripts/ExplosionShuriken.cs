using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionShuriken : MonoBehaviour 
{
	void Destroy()
	{
		//GetComponent<SpriteRenderer> ().sprite = null;
		Destroy (gameObject);
	}
}
