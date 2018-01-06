using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class VirusShip : Ship
{
    private  float glitchTimer;
    private float glitchDelta;

    private void Start()
    {
        glitchTimer = 0F;
        glitchDelta = 0F;
    }

    protected override void virtualUpdate()
    {
        base.virtualUpdate();

        if (!isPossessed())
            this.enabled = false;

        //Glitch animation
        glitchTimer += Time.deltaTime;
        if (glitchTimer >= glitchDelta)
        {
            anim.SetTrigger("glitch");
            glitchTimer = 0F;
            glitchDelta = Random.Range(0, 3f);
        }
    }

    //Makes the ship shoot ! 
    public override void Fire()
    {
    }

    private void OnEnable()
    {
        //Set the VirusShip at the camera position
        Vector3 newPosition = GameManager.MainCameraController.transform.position;
        newPosition.z = 0;
        transform.position = newPosition;


        GetComponent<SpriteRenderer>().enabled = true;
        foreach(Collider2D collider in GetComponents<Collider2D>())
            collider.enabled = true;
        
    }

    private void OnDisable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (Collider2D collider in GetComponents<Collider2D>())
            collider.enabled = false;
    }
}
