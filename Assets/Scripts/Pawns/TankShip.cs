using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankShip : Ship
{
    [Header("Graphic Elements")]
    public SpriteRenderer chargeCircle;
    public SpriteRenderer targetSprite;

    [Header("Charge Parameters")]
    public float maxDistance = 15f;
    public float initialDistance = 1f;
    public float loadDuration = 1;
    public float chargeSpeed = 20F;

    [Header("Charge IA")]
    public ChargePattern pattern;
    private float timerPattern = 0;
    private int currentCharge = 0;

    //Loading charge parameters
    private bool loadingCharge = false;
    private float loadedChargeDistance = 0f;
    private float initialSize = 0f;

    //Charge parameters
    private bool isCharging = false;
    private Vector3 targetCharge;

    private bool oldImmortal;
    private bool oldCanBeStunned;

    // Use this for initialization
    void Start ()
    {
        loadingCharge = false;
        initialSize = chargeCircle.bounds.size.x;

        chargeCircle.enabled = false;
        targetSprite.enabled = false;

        updateChargeCicleSize();

        oldImmortal = health.immortal;
        oldCanBeStunned = canBeStunned;
        if (pattern)
            timerPattern = 1 / pattern.rate;
        else Debug.LogWarning("No pattern for " + name);
    }

    private void FixedUpdate()
    {
        if(isCharging)
        {
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, targetCharge, Time.fixedDeltaTime * chargeSpeed);
            if (rb.transform.position == targetCharge)
                stopCharge();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isPlayerControlled) {
            if (loadingCharge) {
                //Load the ChargeDistance slowly
                loadedChargeDistance += Time.deltaTime * (maxDistance - initialDistance) / loadDuration;
                if (loadedChargeDistance > maxDistance)
                    loadedChargeDistance = maxDistance;
                updateChargeCicleSize();

                //Set the target sprite position
                targetSprite.transform.position = Vector3.MoveTowards(transform.position, getMouseWorldPosition(), loadedChargeDistance);

                rotateTowardTarget(targetSprite.transform.position);
            } else rotateTowardTarget(getMouseWorldPosition());
        } else if(pattern) {
            timerPattern -= Time.deltaTime;

            if (timerPattern < 0) {
                if (currentCharge < pattern.charges.Count) {
                    loadedChargeDistance = pattern.charges[currentCharge].distance;
                    startCharge(transform.position + Quaternion.Euler(0, 0, -(pattern.charges[currentCharge].angle + 180)) * Vector3.up * 20);
                }

                timerPattern = 1 / pattern.rate;
                currentCharge = ++currentCharge % pattern.charges.Count;
            } else if (timerPattern < (1 / pattern.rate) - 0.1f)
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, -(pattern.charges[currentCharge].angle + 180)), transform.rotation, 0.85f);
        }
    }

    //Rotate the ship towards the targetSprite object
    private void rotateTowardTarget(Vector3 target )
    {
        //if( Vector3.Distance(target, transform.position) > 0.01f)
        {
            float sign = (target.x < transform.position.x ? 1.0f : -1.0f);
            float angle = sign * Vector3.Angle(Vector2.up, target - transform.position);

            if( angle != 0 )
                transform.rotation = Quaternion.Euler(0, 0, angle + 180);



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
            startCharge(getMouseWorldPosition());
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

    private void startCharge(Vector3 target)
    {
        isCharging = true;
        health.immortal = true;
        canBeStunned = false;

        //Cap the distance according to the loadedChargeDistance
        targetCharge = Vector3.MoveTowards(transform.position, target, loadedChargeDistance);
    }

    private void stopCharge()
    {
        isCharging = false;
        health.immortal = oldImmortal;
        canBeStunned = oldCanBeStunned;
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
            if (Vector3.Distance(transform.position, getMouseWorldPosition()) > 0.3F)
                transform.position = Vector3.MoveTowards(transform.position, point, Speed * Time.fixedDeltaTime);
    }
}
