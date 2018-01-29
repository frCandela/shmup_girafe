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

	public void SpawnWave(int index, bool spawnLoop)
	{
		_waves [index].SetActive (true);
		if (spawnLoop)
			_waves [index].GetComponent<TutoRespawn> ().enabled = true;
		Debug.Log ("Spawned wave " + index);
	}
}
