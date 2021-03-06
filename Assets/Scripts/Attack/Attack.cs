﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public AttackType attackType;

    private float timerShoot = 0;
    private int currentShoot = 0;

	// Update is called once per frame
	void Update ()
    {
        timerShoot -= Time.deltaTime;
    }

    public void Fire(GameObject shooter, Transform origin)
    {
        if (timerShoot < 0)
        {
            if (currentShoot < attackType.bursts.Count)
            {
                bool attacked = attackType.bursts[currentShoot].Attack(shooter, origin);

                Ship ship = GetComponent<Ship>();
                if (attacked && ship && ship.IsPlayerControlled())
                    FMODUnity.RuntimeManager.PlayOneShot("event:/tir", GameManager.instance.MainCameraController.transform.position);
            }
                

            timerShoot = 1 / attackType.rate;
            currentShoot = ++currentShoot % attackType.bursts.Count;
        }
    }

    public void Reset() {
        currentShoot = 0;
    }
}
