using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class EnemyController : Controller
{
	// Use this for initialization
	void Start ()
    {
        Possess(GetComponent<Pawn>());
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(PossessedPawn)
        {
           // Vector3 direction = PossessedPawn.transform.position - GameManager.instance.MainCameraController.transform.position;
           // float angle = Vector3.Angle(direction.normalized, new Vector3(1, 0, 0)) - 90f;
            PossessedPawn.Fire(Quaternion.Euler(0, 0, 0));
        }
            
    }

}
