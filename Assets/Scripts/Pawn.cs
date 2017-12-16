using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A pawn is a game object that can be possessed
public class Pawn : MonoBehaviour
{
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Reset the the ship behaviour to default
    public void UnPossess()
    {

    }

    //Default pawn actions
    public virtual void MoveHorizontal  ( float axisValue) {}
    public virtual void MoveVertical    ( float axisValue) {}
    public virtual void Fire() {}
}
