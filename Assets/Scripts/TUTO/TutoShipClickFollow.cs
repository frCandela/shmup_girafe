using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoShipClickFollow : MonoBehaviour
{
	[SerializeField] private Transform _ship;
	[SerializeField] private Transform _leftClick;
	[SerializeField] private Transform _rightClick;
	private TutoManager _tuto;
	private Vector3 _shipPos;

	void Start()
	{
		_tuto = GameObject.Find ("TutorialManager").GetComponent<TutoManager> ();
	}

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
