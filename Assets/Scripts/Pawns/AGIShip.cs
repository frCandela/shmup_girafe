using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGIShip : Ship
{
    [Header("AGI ship special parameters:")]
    public Transform cannonPosition;
    public Transform cannonPosition2;

    protected Attack attack2;

    // Use this for initialization
    void Start ()
    {
        Attack[] attacks = GetComponents<Attack>();
        attack = attacks[0];
        attack2 = attacks[1];
    }

    // Update is called once per frame
    public override void Fire(Quaternion angle)
    {
        if ( ! IsStunned() )
        {
            if (anim)
                anim.SetTrigger("Shoot");


            cannonPosition.Rotate(angle.eulerAngles);
            cannonPosition2.Rotate(angle.eulerAngles);

            attack.Fire(  this.gameObject, cannonPosition);
            attack2.Fire( this.gameObject, cannonPosition2);

            cannonPosition.Rotate(-angle.eulerAngles);
            cannonPosition2.Rotate(-angle.eulerAngles);
        }
    }
}
