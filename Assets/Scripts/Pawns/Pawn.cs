using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A pawn is a game object that can be possessed
public class Pawn : MonoBehaviour
{
    public Controller controller/* { get; private set; }*/;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}



    public void setPossessed(Controller newController)
    {
        controller = newController;
    }    

    //Return true if the controller is possessed
    public bool isPossessed() { return controller; }

    //Default pawn actions
    public virtual void MoveHorizontal  ( float axisValue) {}
    public virtual void MoveVertical    ( float axisValue) {}
    public virtual void Fire() {}
}
