using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoEnnemySpawner : MonoBehaviour 
{
	[SerializeField] private GameObject[] _waves;

	void Start()
	{
		foreach (GameObject wave in _waves)
			wave.SetActive (false);
	}

	public void SpawnWave(int index)
	{
		_waves [index].SetActive (true);
		Debug.Log ("Spawned wave " + index);
	}
}
