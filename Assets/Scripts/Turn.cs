using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour {

    public float turnSpeed = 3f;

	void Update () {
        transform.position += new Vector3(0, GameManager.instance.getTunnelSpeed() * Time.deltaTime, 0);
        if(GameManager.instance.PlayerController.PossessedPawn)
            transform.Rotate(new Vector3(0, turnSpeed * -GameManager.instance.PlayerController.PossessedPawn.transform.position.x * Time.deltaTime, 0));
	}
}
