using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A basic pawn for a spaceship in the game
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Attack))]
public class Ship : Pawn
{
    [Header("Ship parameters:")]
    [Range(0.0f, 20.0f)]public float Speed = 0.5f;  //Movement speed of the ship

    private Rigidbody2D rb;

    //Stun
    public bool canBeStunned = true;
    private float stunTimer;
    private bool stunned;

    protected Animator anim;
    protected Attack attack;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
            throw new Exception("Error : Rigidbody2D not set");
        rb.gravityScale = 0f;

        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        attack = GetComponent<Attack>();
        stunTimer = 0F;
        stunned = false;
    }

    public bool IsStunned() { return stunned;  }
    public void Stun( float stunDuration )
    {
        if(canBeStunned)
        {
            stunTimer = stunDuration;
            stunned = true;
        }
    }

    protected override void virtualUpdate()
    {
        base.virtualUpdate();

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0)
        {
            stunned = false;
            stunTimer = 0;
        }
    }

    //Moves the Ship horizontally
    public override void MoveHorizontal(float axisValue)
    {
        if ( ! IsStunned())
        {
            Vector2 newPosition = rb.position;
            newPosition.x += axisValue * Speed * Time.fixedDeltaTime;
            rb.position = newPosition;
        }
    }

    //Moves the Ship vertically
    public override void MoveVertical(float axisValue)
    {
        if ( ! IsStunned())
        {
            Vector2 newPosition = rb.position;
            newPosition.y += axisValue * Speed * Time.fixedDeltaTime;
            rb.position = newPosition;
        }
    }

    public override void MoveTowards(Vector3 point)
    {
        if( ! IsStunned() )
            transform.position = Vector3.MoveTowards(transform.position, point, Speed * Time.fixedDeltaTime);
    }

    //Makes the ship shoot ! 
    public override void Fire()
    {
        if( ! IsStunned())
        {
            if (anim)
                anim.SetTrigger("Shoot");
            attack.Fire(this.gameObject, transform);
        }
    }
}
