﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Virus : Ship
{
    private  float glitchTimer;
    private float glitchDelta;
    private bool wrongSoundPlayed;

	public bool CheckPossessed { get; set;}

    private void Start()
    {
        glitchTimer = 0F;
        glitchDelta = 0F;
        wrongSoundPlayed = false;

        transform.position = GameManager.instance.getMouseWorldPosition();
    }

    protected override void Update()
    {
        base.Update();
        if (CheckPossessed && !isPossessed()) 
		{
			this.enabled = false;
			trail.enabled = false;
			line.enabled = false;
			CheckPossessed = false;
		}

        //Glitch animation
        glitchTimer += Time.deltaTime;
		if (glitchTimer >= glitchDelta)
		{
			anim.SetTrigger ("glitch");
			anim.speed = Random.Range (1f, 3f);
			glitchTimer = 0F;
			glitchDelta = Random.Range (0, 3f);
		} 
    }



    //Makes the ship shoot ! 
    public override void Fire(Quaternion angle)
    {
        //Play a sound when the player tries to Fire
        if (!GameManager.instance.soundManager.wrong.IsPlaying() &&  ! wrongSoundPlayed)
        {
            wrongSoundPlayed = true;
            if(GameManager.instance._playWrong)GameManager.instance.soundManager.wrong.Play();
       	}	
    }

    //When the player realease the FIRE key
    public override void UnFire()
    {
        base.UnFire();
        wrongSoundPlayed = false;
    }


    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        foreach(Collider2D collider in GetComponents<Collider2D>())
            collider.enabled = true;
    }

    private void OnDisable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (Collider2D collider in GetComponents<Collider2D>())
            collider.enabled = false;
        GetComponent<Blink>().StopBlink();
    }

    public override void takeDamage(int damage)
    {
        Blink blink = GetComponent<Blink>();
        if (blink)
            blink.StartBlink();
        if (controller)
            controller.onTakeDamage.Invoke();
    }

    public override void Destroy()
    {
    }

	void SetAnimSpeed()
	{
		anim.speed = 0.15f;
	}
}
