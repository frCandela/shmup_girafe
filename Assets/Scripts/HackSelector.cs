using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The hack selector is used to select and controll a ship in the camera view area
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class HackSelector : MonoBehaviour
{

    [Range(0, 10)] public float HackDuration = 3F;
    [Range(0, 10)] public float HackMinimalDuration = 0.5F;
    [Range(0F, 1F)] public float TimeScaleFactor = 0.1F;

    private SpriteRenderer spriteRenderer;

    private Ship targetShip;
    private Controller targetController;

    private float timeElapsedHack;

    // Use this for initialization
    void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void startHack( Controller controller )
    {
        //If the HackSelector is not hacking
        if ( ! targetShip)
        {
            TimeManager.doSlowMotion(HackDuration, TimeScaleFactor);
            GameManager.instance.ToogleHackEffect();
            targetShip = (Ship)controller.PossessedPawn;
            targetController = controller;

            spriteRenderer.enabled = true;
            timeElapsedHack = 0F;
        }
    }

    public void disable()
    {
        spriteRenderer.enabled = false;
        targetShip = null;
        targetController = null;
    }

    void Update ()
    {
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
                GameManager.instance.ToogleHackEffect();
                disable();
            }
                

            //HACK !
            if (Input.GetButtonDown("Hack") && timeElapsedHack >= HackMinimalDuration)
            {
                targetShip.gameObject.tag = this.gameObject.tag;
                targetShip.UnPossess();
                targetController.Possess(targetShip);
                targetShip.transform.rotation = Quaternion.Euler(0F,0F,0F);
                TimeManager.resetSlowMotion();
                GameManager.instance.ToogleHackEffect();
                disable();
            }
        }
    }

    // Returns true if the HackSelector is hacking
    public bool isHacking(){ return targetShip; }
}
