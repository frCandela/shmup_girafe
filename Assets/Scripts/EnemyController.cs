using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class EnemyController : Controller {

	// Use this for initialization
	void Start () {
        Possess(GetComponent<Pawn>());
	}
	
	// Update is called once per frame
	void Update () {
        PossessedPawn.Fire();
    }

}
