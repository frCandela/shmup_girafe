using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoShipClickFollow : MonoBehaviour
{
	[SerializeField] private Transform _ship;
	[SerializeField] private Transform _leftClick;
	[SerializeField] private Transform _rightClick;
	private Vector3 _shipPos;

	void Update ()
	{
		if (_ship) 
		{
			_shipPos = _ship.position;
		}
		else
			Destroy (this.gameObject);
		_leftClick.position = _shipPos;
		_rightClick.position = _shipPos;
	}
}
