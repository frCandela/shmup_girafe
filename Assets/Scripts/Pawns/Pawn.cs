using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A pawn is a game object that can be possessed
public class Pawn : MonoBehaviour
{
    public Controller controller { get; private set; }

    [Header("Hack parameters:")]
    public bool isHackable = true;
    [Range(0.0f, 100.0f)] public float hackCost = 100F;
    [Range(0.0f, 100.0f),Tooltip("Hack power gained by the player when the ships is destroyed")]
    public float hackbonus = 10F;

    protected virtual void Update()
    {
    }

    public virtual void takeDamage(int damage)
    {
        if(controller)
            controller.onTakeDamage.Invoke();
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
    public virtual void MoveTowards(Vector3 point){}


    public virtual void Fire ( Quaternion angle ) {}
    public virtual void UnFire() {}
}
