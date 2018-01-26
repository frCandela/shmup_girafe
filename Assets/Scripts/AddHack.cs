using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHack : MonoBehaviour {

    public float refillSpeed = 1f;
    
    void Update () {
        if(GetComponent<Ship>().controller == GameManager.instance.PlayerController)
            GameManager.instance.PlayerController.addHackPower(GameManager.instance.PlayerController.virusHackRefillSpeed * Time.deltaTime * refillSpeed);
	}
}
