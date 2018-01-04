using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The hack selector is used to select and controll a ship in the camera view area
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class HackSelector : MonoBehaviour
{
    [Header("Hack parameters:")]
    [Range(0, 10)] public float HackDuration = 3F;
    [Range(0, 10)] public float HackMinimalDuration = 0.5F;
    [Range(0F, 1F)] public float TimeScaleFactor = 0.1F;
    [Range(0F, 100F)] public float hackMissPenalty = 10;

    [Tooltip("In Units peer second"), Range(0F, 10F)]
    public float refillSpeed = 1F;

    private SpriteRenderer spriteRenderer;

    private Ship targetShip;
    private Controller targetController;


    private float timeElapsedHack;
    private float hackPower;
    private float maxHackPower = 100F;
    


    // Use this for initialization
    void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hackPower = 0;
    }

    private void Start()
    {
        spriteRenderer.enabled = false;
        targetShip = null;
        targetController = null;
    }

    public void startHack( Controller controller )
    {
        //If the HackSelector is not hacking
        if ( ! targetShip )
        {
            TimeManager.doSlowMotion(HackDuration, TimeScaleFactor);
            GameManager.instance.setHackEffect(true);
            targetShip = (Ship)controller.PossessedPawn;
            targetController = controller;

            spriteRenderer.enabled = true;
            timeElapsedHack = 0F;
        }
    }

    public void disable()
    {
        TimeManager.resetSlowMotion();
        GameManager.instance.setHackEffect(false);

        spriteRenderer.enabled = false;
        targetShip = null;
        targetController = null;
    }

    void Update ()
    {
        //Refill the hack bar
        hackPower += refillSpeed * Time.deltaTime;
        if (hackPower > maxHackPower)
            hackPower = maxHackPower;

        //If the HackSelector is hacking
        if (targetShip)
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                if (Input.GetAxisRaw("Horizontal") > 0F)
                    targetShip = GameManager.MainCameraController.shipsInCameraView.rightShipFrom(targetShip);
                else
                    targetShip = GameManager.MainCameraController.shipsInCameraView.leftShipFrom(targetShip);
            }
            else if (Input.GetButtonDown("Vertical"))
            {
                if (Input.GetAxisRaw("Vertical") > 0F)
                    targetShip = GameManager.MainCameraController.shipsInCameraView.upperShipFrom(targetShip);
                else
                    targetShip = GameManager.MainCameraController.shipsInCameraView.lowerShipFrom(targetShip);
            }

            //Set the hack selector position
            transform.position = targetShip.transform.position;

            //Updates the hack time
            timeElapsedHack += Time.unscaledDeltaTime;

            //Hack duration reached without pressing the hack key again
            if (timeElapsedHack >= HackDuration)
            {
                //Penalty when the hack is missed
                hackPower -= hackMissPenalty;
                if (hackPower <= 0)
                    hackPower = 0;
                disable();
            }
                

            //HACK !
            if (Input.GetButtonDown("Hack") && timeElapsedHack >= HackMinimalDuration)
            {
                if(targetShip.isHackable && hackPower >= targetShip.hackCost)
                {
                    //misc
                    hackPower -= targetShip.hackCost;
                    GameManager.MainBar.health = targetShip.GetComponent<Health>();

                    //Destroy the old pawn
                    Health oldHealth = targetController.PossessedPawn.GetComponent<Health>();
                    if (!oldHealth || !oldHealth.immortal)//Don't destroy immortal objects 
                        Destroy(targetController.PossessedPawn.gameObject);

                    //Possess the new ship
                    targetShip.gameObject.tag = this.gameObject.tag;
                    targetController.Possess(targetShip);
                    targetShip.transform.rotation = Quaternion.Euler(0F, 0F, 0F);
                }
                else
                {
                    //Penalty when the hack is missed
                    hackPower -= hackMissPenalty;
                    if (hackPower <= 0)
                        hackPower = 0;
                }

                //Reset managers
                disable();
            }
        }
    }

    // Returns true if the HackSelector is hacking
    public bool isHacking(){ return targetShip; }
    public float getHackPowerRatio() { return hackPower/ maxHackPower; }
}
