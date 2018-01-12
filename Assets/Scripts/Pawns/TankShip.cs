using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankShip : Ship
{
    [Header("Graphic Elements")]
    public SpriteRenderer chargeCircle;
    public SpriteRenderer targetSprite;

    [Header("chargeParameters")]
    public float maxDistance = 15f;
    public float initialDistance = 1f;
    public float loadDuration = 1;
    public float chargeSpeed = 20F;

    //Loading charge parameters
    private bool loadingCharge = false;
    private float loadedChargeDistance = 0f;
    private float initialSize = 0f;
    private float chargeTimer = 0f;

    //Charge parameters
    private bool isCharging = false;
    private Vector3 targetCharge;
    private bool wasImmortal;


    // Use this for initialization
    void Start ()
    {
        loadingCharge = false;
        initialSize = chargeCircle.bounds.size.x;

        chargeCircle.enabled = false;
        targetSprite.enabled = false;

        updateChargeCicleSize();

        wasImmortal = health.immortal;
    }

    private void FixedUpdate()
    {
        if(isCharging)
        {
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, targetCharge, Time.fixedDeltaTime * chargeSpeed);
            
            if(rb.transform.position == targetCharge)
                stopCharge();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (loadingCharge)
        {
            //Load the ChargeDistance slowly
            loadedChargeDistance += Time.deltaTime * (maxDistance - initialDistance) / loadDuration;
            if (loadedChargeDistance > maxDistance)
                loadedChargeDistance = maxDistance;
            updateChargeCicleSize();

            //Set the target sprite position
            targetSprite.transform.position = Vector3.MoveTowards(transform.position, getMouseWorldPosition(), loadedChargeDistance);
        }

    }

    //When the player press the FIRE key
    public override void Fire()
    {
        //Initialize the charge
        if( ! loadingCharge && ! isCharging)
        {
            startLoadingCharge();
        }
    }

    //When the player realease the FIRE key
    public override void UnFire()
    {
        if(loadingCharge)
        {
            startCharge();
            stopLoadingCharge();
        }
    }

    private void startLoadingCharge()
    {
        loadingCharge = true;
        loadedChargeDistance = initialDistance;

        chargeCircle.enabled = true;
        targetSprite.enabled = true;

        targetSprite.transform.position = transform.position;
        updateChargeCicleSize();
    }

    private void stopLoadingCharge()
    {
        loadingCharge = false;
        loadedChargeDistance = 0;
        chargeCircle.enabled = false;
        targetSprite.enabled = false;
        updateChargeCicleSize();
    }

    //Return the position of the mouse in world coordinates
    private Vector3 getMouseWorldPosition()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = transform.position.z - Camera.main.transform.position.z; //distance of the plane from the camera
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPoint);
        mousePos.z = transform.position.z;

        return mousePos;
    }

    private void startCharge()
    {
        isCharging = true;
        health.immortal = true;

        //Cap the distance according to the loadedChargeDistance
        targetCharge = Vector3.MoveTowards(transform.position, getMouseWorldPosition(), loadedChargeDistance);
    }

    private void stopCharge()
    {
        isCharging = false;
        health.immortal = wasImmortal;
    }

    //Updates the scale of the charge circle
    private void updateChargeCicleSize()
    {
        float newScale = 2 * loadedChargeDistance / initialSize;
        chargeCircle.transform.localScale = new Vector3(newScale, newScale, newScale);
    }


    public override void MoveTowards(Vector3 point)
    {
        if (!IsStunned() && !loadingCharge && !isCharging)
            transform.position = Vector3.MoveTowards(transform.position, point, Speed * Time.fixedDeltaTime);
    }
}
