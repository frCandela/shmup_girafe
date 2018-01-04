using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The player controller that can possess pawns
public class KeyboardController : Controller
{
    public HackSelector HackSelectorPrefab;
    public HackSelector hackSelector { get; private set; }

    public Ship VirusShipPrefab;
    private Ship virusShip;

    // Use this for initialization
    void Awake()
    {
        //Initialisation checks
        if (!HackSelectorPrefab)
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : no HackSelectorPrefab selected");
        }
        //Initialisation checks
        if (!VirusShipPrefab)
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : no VirusShipPrefab selected");
        }
        //Instanciates the hack selector
        hackSelector = Instantiate(HackSelectorPrefab, transform.position, transform.rotation);
        virusShip = Instantiate(VirusShipPrefab, transform.position, transform.rotation);
        
        virusShip.enabled = false;
    }

    private void Start()
    {
        //Set the health bar
        GameManager.MainBar.health = PossessedPawn.GetComponent<Health>();
        GameManager.MainBar.hackSelector = hackSelector;
    }

    private void Update()
    {
        if (isPossessingPawn())
        {
            //input
            if (Input.GetButton("Fire"))
                PossessedPawn.Fire();
            if (Input.GetButtonDown("Hack"))
                hackSelector.startHack(this);
                
            //The ship cannot go out of the camera fov
            GameManager.MainCameraController.snapInCameraView(PossessedPawn);
        }
        else
        {
            hackSelector.disable();
            //Set the player to a virus
            virusShip.enabled = true;
            GameManager.MainBar.health = virusShip.GetComponent<Health>();
            Possess(virusShip);
        }

        //Update the hack bar
        GameManager.MainBar.setHackBar(hackSelector.getHackPowerRatio());
    }

    void FixedUpdate()
    {
        //Handle player actions if a pawn is possessed
        if (isPossessingPawn() && ! hackSelector.isHacking() )
        {
            PossessedPawn.MoveHorizontal(Input.GetAxisRaw("Horizontal"));
            PossessedPawn.MoveVertical(Input.GetAxisRaw("Vertical"));
        }
    }
}
