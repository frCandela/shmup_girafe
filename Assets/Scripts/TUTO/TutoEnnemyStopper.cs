using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoEnnemyStopper : MonoBehaviour
{
	[SerializeField] private bool _shouldStop = true;
	[SerializeField] private float _stopPosition = 12f;

	 void Update()
	{
		if(_shouldStop)
		{
			if (transform.position.y < _stopPosition) 
			{
				if (GetComponent<DPSShip> ())
					GetComponent<DPSShip> ().scrollingSpeed = 0;
				else if (GetComponent<TankShip> ())
					GetComponent<TankShip> ().Stun (60f);
			}
		}
	}
}
