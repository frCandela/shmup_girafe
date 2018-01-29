using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A basic pawn for a spaceship in the game
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]

public class Ship : Pawn
{
    [Header("Prefabs:")]
    public GameObject explosionPrefab;

    [Header("Parameters")]
    [Range(0.0f, 100.0f)]public float playerSpeed = 0.5f;  //Movement speed of the ship

    //Stun
    public bool canBeStunned = true;
    private bool isPlayerControlled = false;

    //Speed
    public float scrollingSpeed;

    private float stunTimer;
    private bool stunned;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Attack attack;
    protected Health health;
    protected SpriteRenderer sprite;

    // Use this for initialization
    void Awake()
    {
        //Set components
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
            throw new Exception("Error : Rigidbody2D not set");
        rb.gravityScale = 0f;

        anim = GetComponent<Animator>();
        attack = GetComponent<Attack>();
        health = GetComponent<Health>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        stunTimer = 0F;
        stunned = false;
    }

    public void setHackAnim( bool state){anim.SetBool("Hacked", state);}

    public bool IsStunned() { return stunned;  }
    public void Stun( float stunDuration )
    {
        if(canBeStunned)
        {
            stunTimer = stunDuration;
            stunned = true;
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        //Collision with other ships
        Ship ship = collision.gameObject.GetComponent<Ship>();
        if (ship && ship.tag != tag )
        {
           

            Health otherHealth = ship.GetComponent<Health>();
            Health myHealth = GetComponent<Health>();

            if (isPlayerControlled)
            {
                otherHealth.takeDamage(otherHealth.health, this);

				if (myHealth.health == 1 || GetComponent<TankShip>())
					myHealth.takeDamage (1, this);
                else
					myHealth.takeDamage (myHealth.health - 1, this);
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        transform.position += new Vector3(0, scrollingSpeed * Time.deltaTime, 0);

        //Stun
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
            newPosition.x += axisValue * playerSpeed * Time.fixedDeltaTime;
            rb.position = newPosition;
        }
    }

    //Moves the Ship vertically
    public override void MoveVertical(float axisValue)
    {
        if ( ! IsStunned())
        {
            Vector2 newPosition = rb.position;
            newPosition.y += axisValue * playerSpeed * Time.fixedDeltaTime;
            rb.position = newPosition;
        }
    }

    public override void MoveTowards(Vector3 point)
    {
        if( ! IsStunned() )
            transform.position = Vector3.MoveTowards(transform.position, point, playerSpeed * Time.fixedDeltaTime);
    }

    //Makes the ship shoot ! 
    public override void Fire( Quaternion angle ) 
    {
        if( ! IsStunned())
        {
            if (anim)
                anim.SetTrigger("Shoot");

            transform.Rotate(angle.eulerAngles);
            attack.Fire(this.gameObject, transform);
            transform.Rotate( - angle.eulerAngles);
        }
    }

    public override void UnFire() {
        if(attack)
            attack.Reset();
    }

    protected void OnDestroy()
    {
        /*GameManager.instance.PlayerController.addHackPower(hackbonus);
        if( isPlayerControlled)
            FMODUnity.RuntimeManager.PlayOneShot("event:/explosion_player", transform.position);
        else
            FMODUnity.RuntimeManager.PlayOneShot("event:/explosion_enemy", transform.position);*/
    }

    public virtual void Destroy()
    {
        Destroy(this.gameObject);
        if(explosionPrefab)
            Instantiate(explosionPrefab, transform.position, transform.rotation);
		if (!isPlayerControlled)
		{
			Instantiate (GameManager.instance.hackFillerPrefab, transform.position, Quaternion.identity);
			Instantiate (GameManager.instance.hackFillerPrefab, transform.position, Quaternion.identity);
			Instantiate (GameManager.instance.hackFillerPrefab, transform.position, Quaternion.identity);
		}
		GameManager.instance.PlayerController.addHackPower(hackbonus);
		if( isPlayerControlled)
			FMODUnity.RuntimeManager.PlayOneShot("event:/explosion_player", transform.position);
		else
			FMODUnity.RuntimeManager.PlayOneShot("event:/explosion_enemy", transform.position);
    }

	public IEnumerator HackImmunity()
	{
		health.immortal = true;
		yield return new WaitForSeconds (1f);
		health.immortal = false;
	}


    public bool IsPlayerControlled() { return isPlayerControlled; }
    public void SetPlayerControlled( bool value) { isPlayerControlled = value; }
}
