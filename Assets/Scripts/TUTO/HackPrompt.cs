using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackPrompt : MonoBehaviour
{
	[SerializeField] private GameObject _clickToHack;
	private MouseController _mouse;
	private Virus _ship;

	void Start()
	{
		_mouse = GameManager.instance.GetComponent<GameManager> ().PlayerController;
		_ship = GetComponent<Virus> ();
		_clickToHack.SetActive (false);
	}

	void Update()
	{
		if(_ship.isActiveAndEnabled)
		{
			if (_mouse.getHackPowerRatio () > 0.99f)
				_clickToHack.SetActive (true);
			else
				_clickToHack.SetActive (false);
		}
		else
		{
			_clickToHack.SetActive (false);
		}
	}
}
