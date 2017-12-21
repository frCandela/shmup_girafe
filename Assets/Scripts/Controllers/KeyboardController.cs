using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The player controller that can possess pawns
public class KeyboardController : Controller
{
    public HackSelector HackSelectorPrefab;
    private HackSelector hackSelector;

    // Use this for initialization
    void Awake()
    {
        //Initialisation checks
        if (!HackSelectorPrefab)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : no HackSelectorPrefab selected");
        }

        //Instanciates the hack selector
        hackSelector = Instantiate(HackSelectorPrefab, transform.position, transform.rotation) as HackSelector;
    }

    private void Start()
    {
        hackSelector.disable();
    }

    private void Update()
    {
        if (isPossessingPawn())
        {
            if (Input.GetButton("Fire"))
                PossessedPawn.Fire();

            if (Input.GetButtonDown("Hack"))
                hackSelector.startHack(this);
                
            GameManager.MainCameraController.snapInCameraView(PossessedPawn);
        }
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
