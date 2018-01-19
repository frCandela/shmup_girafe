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

    protected Attack attack2;
    protected Attack attack3;
    protected Attack attack4;

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
        if ( ! IsStunned())
        {
            if (anim)
                anim.SetTrigger("Shoot");

            attack.Fire(this.gameObject, cannonPosition);
            attack2.Fire(this.gameObject, cannonPosition2);
            attack3.Fire(this.gameObject, cannonPosition3);
            attack4.Fire(this.gameObject, cannonPosition4);
        }
    }

    public override void UnFire()
    {
        attack.Reset();
        attack2.Reset();
        attack3.Reset();
        attack4.Reset();
    }

}
