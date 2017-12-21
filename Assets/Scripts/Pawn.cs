using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A pawn is a game object that can be possessed
public class Pawn : MonoBehaviour
{
    private Controller controller;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}



    public void Possess(Controller newController)
    {
        controller = newController;
    }    
    
    //Reset the the ship behaviour to default
    public void UnPossess()
    {
        if(controller)
        {
            controller.UnPossess();
            controller = null;
        }
    }

    //Default pawn actions
    public virtual void MoveHorizontal  ( float axisValue) {}
    public virtual void MoveVertical    ( float axisValue) {}
    public virtual void Fire() {}
}
