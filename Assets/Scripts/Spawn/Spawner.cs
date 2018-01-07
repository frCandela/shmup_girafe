using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public float scale = 8;
    public float offset = 13;

	public void Spawn (GameObject prefab, Vector2 pos) {
        Instantiate(prefab, new Vector3(pos.x * scale, transform.position.y + offset - pos.y * scale, 0), Quaternion.Euler(0,0,180));
    }
}
