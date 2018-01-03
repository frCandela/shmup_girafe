using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A controller is a script used to control a pawn
public class Controller : MonoBehaviour
{
    //The pawn the controller is possessing
    public Pawn PossessedPawn/* { get; private set; }*/;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Returns true if a pawn is possessed, false otherwise
    public bool isPossessingPawn() { return PossessedPawn != null;}

    //Makes the controller possess a new pawn
    public void Possess(Pawn newPawn)
    {
        if (newPawn.isPossessed())
            newPawn.controller.UnPossess();
        if(PossessedPawn)
            PossessedPawn.setPossessed(null);
        PossessedPawn = newPawn;
        newPawn.setPossessed(this);
    }

    public void UnPossess()
    {
        PossessedPawn = null;
    }
}
