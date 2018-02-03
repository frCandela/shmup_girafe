using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour 
{
	private GameObject _player;

	public void SetPlayerPosition(GameObject player)
	{
		_player = player;
	}

	void Update ()
	{
		if(_player)
		{
			transform.position = _player.transform.position;
		}
	}
}
