using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public void Spawn (GameObject prefab, Vector2 pos) {
        Debug.Log(prefab);
        Debug.Log(pos);
        Instantiate(prefab, new Vector3(pos.x * 7, transform.position.y + 10 - pos.y * 7, 0), Quaternion.Euler(0,0,180));
    }
}
