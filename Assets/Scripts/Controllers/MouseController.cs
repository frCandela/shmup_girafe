using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseController : Controller
{
    public float screenLimit = 7f;

    [Header("GameObjects")]
    public Virus VirusShipPrefab;
    private Virus virusShip;
    public GameObject hackPointerPrefab;
    public GameObject hackPointer;

    [Header("Hack parameters:")]
    public float HackDuration = 3F;
    public bool infiniteDuration = false;
    public float HackSnapDistance = 1F;
    [Range(0F, 1F)] public float TimeScaleFactor = 0.1F;
    [Range(0F, 100F)] public float hackRefillSpeed = 1F;
    [Range(0F, 100F)] public float virusHackRefillSpeed = 15F;

    [Header("Other parameters:")]
    public bool shootVertically = true;

    //Events
    public UnityEvent onHackStart;
    public UnityEvent onHackStop;
    public UnityEvent onHack;
    public UnityEvent onBecomeVirus;

    //Private hack parameters
    private float hackPower = 0;
    private float maxHackPower = 100F;
    private float minHackPower = 100F;
    private bool isHacking = false;
    private Ship targetHack;
    private bool HackSoundTrigerred = false;

    private void Awake()
    {
        virusShip = Instantiate(VirusShipPrefab, transform.position, transform.rotation);
    }

    public bool hacking() {
        return isHacking;
    }

    public Ship getTargetHack()
    {
        return targetHack;
    }

    private void Start()
    {   
        //Hack parameters
        isHacking = false;
        if (infiniteDuration)
            HackDuration = float.MaxValue;
        targetHack = null;

        //hackPointer
        hackPointer = Instantiate(hackPointerPrefab, transform.position, transform.rotation);
        hackPointer.GetComponent<SpriteRenderer>().enabled = false;

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
        if (!virusShip)
            print("NO VIRUS");

        //Refill the hack bar
        if ( PossessedPawn == virusShip )
            hackPower += virusHackRefillSpeed * Time.deltaTime;
        else
            hackPower += hackRefillSpeed * Time.deltaTime;

        if (hackPower >= maxHackPower)
        {
            hackPower = maxHackPower;
            if( !HackSoundTrigerred)
            {
                HackSoundTrigerred = true;
                FMODUnity.RuntimeManager.PlayOneShot("event:/jaugeHackPleine", GameManager.instance.MainCameraController.transform.position);
            }
        }
            

        //If the ship is destroyed, control the virus ship
        if (!isPossessingPawn())
        {
            if (isHacking)
            {
                TimeManager.resetSlowMotion();
                onHackStop.Invoke();
                isHacking = false;
                if (GameManager.instance.soundManager.hackSurvol.IsPlaying())
                    GameManager.instance.soundManager.hackSurvol.Stop();
            }

            this.Invoke( "PossessVirus", 0.1f);
            onBecomeVirus.Invoke();
            hackPower = 0;
        }

        if( isHacking )
        {
            targetHack = null;

            foreach (var s in GameManager.instance.MainCameraController.shipsInCameraView)
            {
                if(s.Value && Vector3.Distance(s.Value.transform.position, GameManager.instance.getMouseWorldPosition()) < HackSnapDistance)
                {
                    targetHack = s.Value;
                    break;
                }
            }
            if (targetHack)
            {
                hackPointer.transform.position = targetHack.transform.position;

                if( ! GameManager.instance.soundManager.hackSurvol.IsPlaying())
                    GameManager.instance.soundManager.hackSurvol.Play();
            }
            else
            {
                hackPointer.transform.position = GameManager.instance.getMouseWorldPosition();
                if (GameManager.instance.soundManager.hackSurvol.IsPlaying())
                    GameManager.instance.soundManager.hackSurvol.Stop();
            }  
        }



        if (Input.GetButton("Fire"))
        {
            if (isHacking)
            {
                isHacking = false;

                if (targetHack && targetHack.hackCost <= hackPower && targetHack != PossessedPawn && targetHack.isHackable)
                {
                    //misc
                    HackSoundTrigerred = false;
                    hackPower -= targetHack.hackCost;
                    if (hackPower < 0F)
                        hackPower = 0F;
                    GameManager.instance.MainBar.health = targetHack.GetComponent<Health>();

                    //Destroy the old pawn
                    PossessedPawn.hackbonus = 0;
                    ((Ship)PossessedPawn).Destroy();


                    //Possess the new ship
                    targetHack.gameObject.tag = this.gameObject.tag;
                    this.Possess(targetHack);
                    targetHack.transform.rotation = Quaternion.Euler(0F, 0F, 0F);
                    targetHack.SetPlayerControlled(true);
                    hackPointer.GetComponent<ParticleSystem>().Play();

                    //Reduce hitbox size except for tank ships
                    if (!targetHack.GetComponent<TankShip>()) targetHack.GetComponent<CapsuleCollider2D>().size /= 2;

                    //Set events
                    targetHack.GetComponent<Health>().onTakeDamage.AddListener(targetHack.GetComponent<Blink>().StartBlink);

                    //Set Health
                    Health targetHealth = targetHack.GetComponent<Health>();
                    if (targetHealth)
                        targetHealth.RestoreHealth();
                    if( targetHealth.health == 1 )
                        FMODUnity.RuntimeManager.PlayOneShot("event:/lowhealth", GameManager.instance.MainCameraController.transform.position);

                    //No score gained when possessed ship is destroyed
                    Score score = targetHack.GetComponent<Score>();
                    if (score)
                        Destroy(score);

                    //Set anim
                    targetHack.setHackAnim(true);

                    targetHack.scrollingSpeed = 0F;

					//Make the ship immune for a second
					StartCoroutine (targetHack.HackImmunity ());

                    onHack.Invoke();
                }
                else {
					onHackStop.Invoke ();
					GameManager.instance.MainBar.ShowHackMessage ();
				}

                if (GameManager.instance.soundManager.hackSurvol.IsPlaying())
                    GameManager.instance.soundManager.hackSurvol.Stop();

                TimeManager.resetSlowMotion();
                //hackPointer.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                float angle = 0f;
                //Angle to shoot verticaly camerawise
                if (shootVertically)
                {
                    Vector3 direction = PossessedPawn.transform.position - GameManager.instance.MainCameraController.transform.position;
                    angle = Vector3.Angle(direction.normalized, new Vector3(1, 0, 0)) - 90f;
                }
                PossessedPawn.Fire(Quaternion.Euler(0, 0, angle / 2));
            }
                
        }

        if (Input.GetButtonUp("Fire"))
            PossessedPawn.UnFire();

        //Hack timeout
        if (isHacking && !TimeManager.inSlowMotion())
        {
            isHacking = false;
            //hackPointer.GetComponent<SpriteRenderer>().enabled = false;
            onHackStop.Invoke();
        }

        //Starts the hack !
        if (Input.GetButtonDown("Hack"))
        {
            if(isHacking)
            {
                onHackStop.Invoke();

                if (GameManager.instance.soundManager.hackSurvol.IsPlaying())
                    GameManager.instance.soundManager.hackSurvol.Stop();

                TimeManager.resetSlowMotion();

				GameManager.instance.MainBar.ShowHackMessage ();
            }
            else if (hackPower >= minHackPower)
            {
                isHacking = true;
				StartCoroutine(GameManager.instance.MainBar.HideHackMessage ());
                //hackPointer.GetComponent<SpriteRenderer>().enabled = true;
                TimeManager.doSlowMotion(HackDuration, 0.05f);
                onHackStart.Invoke();
            }
			else FMODUnity.RuntimeManager.PlayOneShot("event:/wrong", GameManager.instance.MainCameraController.transform.position);

        }
    }

    internal void PossessVirus()
    {
        GameManager.instance.MainBar.health = virusShip.GetComponent<Health>();
        virusShip.enabled = true;
        virusShip.transform.position = GameManager.instance.getMouseWorldPosition();
        Possess(virusShip);
    }

    public bool isVirus() { return virusShip.enabled; }

    public float getHackPowerRatio() { return hackPower / maxHackPower; }
    public void addHackPower( float value )
    {
        hackPower += value;
		if (hackPower > maxHackPower)
			hackPower = maxHackPower;
		else if (hackPower < 0f)
			hackPower = 0f;
    }

    /*public void resetHack()
    {
        hackPower = 0;
    }*/

    void FixedUpdate()
    {
        //Moves the pawn towards the mouse position
        if (isPossessingPawn() &&  ! isHacking )
            PossessedPawn.MoveTowards( GameManager.instance.getMouseWorldPosition() );
    }
}
