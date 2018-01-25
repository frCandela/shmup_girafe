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
    public float deltaRotationSpeed = 5f;

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

    private bool oldCanBeStunned;


    // Use this for initialization
    void Start ()
    {


        loadingCharge = false;
        initialSize = chargeCircle.bounds.size.x;

        chargeCircle.enabled = false;
        targetSprite.enabled = false;

        updateChargeCicleSize();

        oldCanBeStunned = canBeStunned;
        if (pattern)
            timerPattern = 1 / pattern.rate;
        else Debug.LogWarning("No pattern for " + name);

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
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
        if (IsPlayerControlled())
        {
            if (loadingCharge)
            {
                //Load the ChargeDistance slowly
                loadedChargeDistance += Time.deltaTime * (maxDistance - initialDistance) / loadDuration;
                if (loadedChargeDistance > maxDistance)
                    loadedChargeDistance = maxDistance;
                updateChargeCicleSize();

                //Set the target sprite position
                targetSprite.transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.getMouseWorldPosition(), loadedChargeDistance);

                rotateTowardTarget(targetSprite.transform.position);
            }
            else if( isCharging )
            {

            }
            else
            {
                //Rotates the character back 
                Vector3 rotation = transform.rotation.eulerAngles;
                rotation = Vector3.RotateTowards(rotation, new Vector3(0, 0, 180), deltaRotationSpeed, deltaRotationSpeed);
                transform.rotation = Quaternion.Euler( rotation );

            }
                
        }
        else if(pattern)
        {
            timerPattern -= Time.deltaTime;

            if (timerPattern < 0)
            {
                if (currentCharge < pattern.charges.Count)
                {
                    anim.SetBool("Charging", true);
                    loadedChargeDistance = pattern.charges[currentCharge].distance;
                    startCharge(transform.position + Quaternion.Euler(0, 0, -(pattern.charges[currentCharge].angle + 180)) * Vector3.up * 20);
                }
                
                timerPattern = 1 / pattern.rate;
                currentCharge = ++currentCharge % pattern.charges.Count;
            }
            else if (timerPattern < (1 / pattern.rate) - 0.1f)
            {
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, -(pattern.charges[currentCharge].angle + 180)), transform.rotation, 0.85f);
            }
            else
                anim.SetBool("Charging", false);

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
    public override void Fire(Quaternion angle)
    {
		if(loadingCharge)
		{
			anim.SetBool("Charging", true);
            FMODUnity.RuntimeManager.PlayOneShot("event:/chargetank", transform.position);
            startCharge(GameManager.instance.getMouseWorldPosition());
			stopLoadingCharge();
		}

    }

    //When the player realease the FIRE key
    public override void UnFire()
    {
		startLoadingCharge ();
		anim.SetBool("Charging", false);
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

    private void startCharge(Vector3 target)
    {
        isCharging = true;
        health.immuneToShipCollisions = true;
        canBeStunned = false;

        //Cap the distance according to the loadedChargeDistance
        targetCharge = Vector3.MoveTowards(transform.position, target, loadedChargeDistance);
    }

    private void stopCharge()
    {
        isCharging = false;
        health.immuneToShipCollisions = false;
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
            if (Vector3.Distance(transform.position, GameManager.instance.getMouseWorldPosition()) > 0.3F)
                transform.position = Vector3.MoveTowards(transform.position, point, playerSpeed * Time.fixedDeltaTime);
    }
}
