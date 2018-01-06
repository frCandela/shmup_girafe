using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attack))]

public class DPSShip : Ship
{
    [Header("DPS ship special parameters:")]
    public Transform cannonPosition;
    public Transform cannonPosition2;
    public Transform cannonPosition3;
    public Transform cannonPosition4;

    private Attack attack2;
    private Attack attack3;
    private Attack attack4;

    private void Start()
    {
        Attack[] attacks = GetComponents<Attack>();
        attack = attacks[0];
        attack2 = attacks[1];
        attack3 = attacks[2];
        attack4 = attacks[3];
    }

    //Makes the ship shoot ! 
    public override void Fire()
    {
        if (anim)
            anim.SetTrigger("Shoot");

        attack.Fire(this.gameObject, cannonPosition);
        attack2.Fire(this.gameObject, cannonPosition2);
        attack3.Fire(this.gameObject, cannonPosition3);
        attack4.Fire(this.gameObject, cannonPosition4);
    }

}
