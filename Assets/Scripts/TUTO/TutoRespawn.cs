using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoRespawn : MonoBehaviour
{
	[SerializeField] private GameObject _ennemyPrefab;
	[SerializeField] private GameObject _waveEnnemy;
	[SerializeField] private TutoManager _tuto;
	private Vector3 _spawnPosition;
	private Quaternion _spawnRotation;

	void Start()
	{
		_spawnPosition = transform.position;
		_spawnRotation = transform.rotation;
	}

	void Update()
	{
		if (!_waveEnnemy) 
		{
			if (_tuto._spawnAgain)
			{
				GameObject bla = Instantiate (_ennemyPrefab, _spawnPosition, _spawnRotation);
				_waveEnnemy = bla;
			}
		}
	}
}
