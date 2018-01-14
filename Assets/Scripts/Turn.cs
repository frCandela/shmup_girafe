using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour {

    public float speed = 3f;

	void Update () {
        transform.Rotate(new Vector3(0, speed * -GameManager.instance.PlayerController.PossessedPawn.transform.position.x * Time.deltaTime, 0));
	}
}
