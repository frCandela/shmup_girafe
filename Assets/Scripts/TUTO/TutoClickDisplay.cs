using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoClickDisplay : MonoBehaviour 
{
	[SerializeField] private GameObject _leftClickDisplay;
	[SerializeField] private GameObject _rightClickDisplay;
	private TutoManager _tuto;
	[SerializeField]private DPSShip _dps;
	[SerializeField]private TankShip _tank;
	private bool _isDPS = false;

	void Awake()
	{
		_tuto = GameObject.Find ("TutorialManager").GetComponent<TutoManager> ();
		if (_dps != null)
			_isDPS = true;
	}

	void Update ()
	{
		if (_tuto) 
		{
			if (_tuto._hacking) 
			{
				if(_isDPS)
				{
					if(!_dps.isPlayerControlled)
						_leftClickDisplay.SetActive (true);
					else
						_leftClickDisplay.SetActive (false);
				}
				else if (!_tank.isPlayerControlled)
					_leftClickDisplay.SetActive (true);
				else _leftClickDisplay.SetActive (false);
			}
			else
				_leftClickDisplay.SetActive (false);

			if (_tuto._waitingForHack)
			{
				if (_isDPS)
				{
					if (_dps.isPlayerControlled)
						_rightClickDisplay.SetActive (true);
					else _rightClickDisplay.SetActive (false);
				} else if (_tank.isPlayerControlled)
					_rightClickDisplay.SetActive (true);
				else
					_rightClickDisplay.SetActive (false);
			}
			else
				_rightClickDisplay.SetActive (false);
		}
	}
}
