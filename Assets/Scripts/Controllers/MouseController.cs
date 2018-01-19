using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseController : Controller
{
    public float screenLimit = 7f;

    [Header("GameObjects")]
    public Ship VirusShipPrefab;
    private Ship virusShip;

    [Header("Hack parameters:")]
    public float HackDuration = 3F;
    public bool infiniteDuration = false;
    [Range(0F, 1F)] public float TimeScaleFactor = 0.1F;
    [Range(0F, 100F)] public float hackRefillSpeed = 1F;
    [Range(0F, 100F)] public float virusHackRefillSpeed = 1F;

    //Events
    public UnityEvent onHack;
    public UnityEvent onBecomeVirus;

    //Private hack parameters
    private float hackPower = 0;
    private float maxHackPower = 100F;
    private float minHackPower = 100F;
    private bool isHacking = false;

    private void Start()
    {
        //Hack parameters
        isHacking = false;
        if (infiniteDuration)
            HackDuration = float.MaxValue;

        //Virus ship
        if (!virusShip)
            virusShip = Instantiate(VirusShipPrefab, transform.position, transform.rotation);

        //Set events on the possesed ship
        if (PossessedPawn && PossessedPawn != virusShip)
        {
            Ship ship = (Ship)PossessedPawn;
            GameManager.instance.MainBar.health = ship.GetComponent<Health>();
            ship.GetComponent<Health>().onTakeDamage.AddListener(ship.GetComponent<Blink>().StartBlink);
        }
    }

    private void Update()
    {
        //Refill the hack bar
        if( PossessedPawn == virusShip)
            hackPower += virusHackRefillSpeed * Time.deltaTime;
        else
            hackPower += hackRefillSpeed * Time.deltaTime;
        if (hackPower > maxHackPower)
            hackPower = maxHackPower;

        //If the ship is destroyed, control the virus ship
        if (!isPossessingPawn())
        {
            onBecomeVirus.Invoke();
            PossessVirus();
        }

        if (Input.GetButton("Fire"))
        {
            if (isHacking)
            {
                isHacking = false;
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -GameManager.instance.MainCameraController.transform.position.z)), Vector2.zero, Mathf.Infinity, 256, -Mathf.Infinity);
                if (hit && hit.collider)
                {
                    Ship target = hit.collider.gameObject.GetComponent<Ship>();


                    if(target.hackCost <= hackPower && target != PossessedPawn && target.isHackable)
                    {
                        //misc
                        hackPower -= target.hackCost;
                        if (hackPower < 0F)
                            hackPower = 0F;
                        GameManager.instance.MainBar.health = target.GetComponent<Health>();

                        //Destroy the old pawn
                        Health oldHealth = this.PossessedPawn.GetComponent<Health>();
                        if (!oldHealth || !oldHealth.immortal)//Don't destroy immortal objects 
                            Destroy(this.PossessedPawn.gameObject);

                        //Possess the new ship
                        target.gameObject.tag = this.gameObject.tag;
                        this.Possess(target);
                        target.transform.rotation = Quaternion.Euler(0F, 0F, 0F);
                        target.isPlayerControlled = true;

                        //Set events
                        target.GetComponent<Health>().onTakeDamage.AddListener(target.GetComponent<Blink>().StartBlink);

                        //Set Health
                        Health targetHealth = target.GetComponent<Health>();
                        if (targetHealth)
                            targetHealth.RestoreHealth();

                        //Set anim
                        target.setHackAnim(true);

                        onHack.Invoke();

                    }
                }

                TimeManager.resetSlowMotion();
            }
            else
                PossessedPawn.Fire();
        }

        if (Input.GetButtonUp("Fire"))
            PossessedPawn.UnFire();


        if (isHacking && !TimeManager.inSlowMotion()) {
            isHacking = false;
        }

        //Starts the hack !
        if (Input.GetButton("Hack") && !isHacking && hackPower >= minHackPower)
        {
            isHacking = true;
            TimeManager.doSlowMotion(3, 0.05f);
        }
    }

    internal void PossessVirus() {
        if(!virusShip)
            virusShip = Instantiate(VirusShipPrefab, transform.position, transform.rotation);

        GameManager.instance.MainBar.health = virusShip.GetComponent<Health>();
        virusShip.enabled = true;
        virusShip.transform.position = transform.position;
        Possess(virusShip);
    }

    public bool isVirus() { return virusShip.enabled; }

    public float getHackPowerRatio() { return hackPower / maxHackPower; }
    public void addHackPower( float value )
    {
        hackPower += value;
        if (hackPower > maxHackPower)
            hackPower = maxHackPower;
    }

    void FixedUpdate()
    {
        //Moves the pawn towards the mouse position
        if (isPossessingPawn() &&  ! isHacking )
            PossessedPawn.MoveTowards( GameManager.instance.getMouseWorldPosition() );
    }
}
