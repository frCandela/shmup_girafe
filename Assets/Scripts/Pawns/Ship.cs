using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A basic pawn for a spaceship in the game
public class Ship : Pawn
{
    [Range(0.0f, 20.0f)]
    public float Speed = 0.5f;  //Movement speed of the ship

    private Rigidbody2D rb;
    private Animator anim;

    public AttackType attack;
    float timerShoot = 0;
    int currentShoot = 0;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            throw new Exception("Error : Rigidbody2D not set");
        }
        rb.gravityScale = 0f;

        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timerShoot -= Time.deltaTime;
    }

    //Moves the Ship horizontally
    public override void MoveHorizontal(float axisValue)
    {
        Vector2 newPosition = rb.position;
        newPosition.x += axisValue * Speed * Time.fixedDeltaTime;
        rb.position = newPosition;
    }

    //Moves the Ship vertically
    public override void MoveVertical(float axisValue)
    {
        Vector2 newPosition = rb.position;
        newPosition.y += axisValue * Speed * Time.fixedDeltaTime;
        rb.position = newPosition;
    }

    //Makes the ship shoot ! 
    public override void Fire()
    {
        if (timerShoot < 0)
        {
            if (anim)
                anim.SetTrigger("Shoot");
            attack.Attack(this.gameObject, currentShoot);
            timerShoot = 1 / attack.rate;
            currentShoot = ++currentShoot % attack.bursts.Count;
        }
    }

}
