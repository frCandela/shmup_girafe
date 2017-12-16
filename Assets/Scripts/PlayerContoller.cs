using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The player controller that can possess pawns
public class PlayerContoller : Controller
{

    // Use this for initialization
    void Start ()
    {
		
	}
	

	void FixedUpdate ()
    {
        //Handle player actions if a pawn is possessed
        if(isPossessingPawn())
        {
            PossessedPawn.MoveHorizontal(Input.GetAxis("Horizontal"));
            PossessedPawn.MoveVertical(Input.GetAxis("Vertical"));

            if (Input.GetButton("Fire1"))
                PossessedPawn.Fire();
        }
	}
}
