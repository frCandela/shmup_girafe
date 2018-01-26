using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshShip : Ship
{
    [Header("Flesh Parameters")]
    [Range(0, 100)]public int hackRefillSpeed = 15;

    private void Start()
    {
        

    }

    protected override void Update()
    {
        base.Update();

        if( controller is MouseController)
           ( (MouseController)controller).addHackPower(Time.deltaTime * hackRefillSpeed);
    }
}
