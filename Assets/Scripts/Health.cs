using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Properties :")]
    public bool immortal = false;
    public bool immuneToDamage = false;
    public bool immuneToShipCollisions = false;

    [Range(0, 100)] public int health = 10;

    
    //Events
    public UnityEvent onDie;
    public UnityEvent onTakeDamage;

    //Private parameters
    private int maxHealth;
    private bool dead = false;

    public bool isDead() { return dead;  }

    private void Start()
    {
        if (immortal)
        {
            health = 0;
            maxHealth = 0;
        }
        maxHealth = health;
    }

    public void RestoreHealth()
    {
        health = maxHealth;
    }

    //Returns the health scaled between 0 and 1
    public int getMaxHealth()
    {
        return maxHealth;
    }

    public void takeDamage(int damage, Object damageDealer)
    {
        if (immortal)
            return;
        if (immuneToDamage && damageDealer is Damage)
            return;
        if (immuneToShipCollisions && damageDealer is Ship)
            return;

        onTakeDamage.Invoke();
        health -= damage;

        //Hit sound
        if( !GameManager.instance.soundManager.hitEnnemies.IsPlaying())
            GameManager.instance.soundManager.hitEnnemies.Play();

        //FMODUnity.RuntimeManager.PlayOneShot("event:/hit", transform.position);

        if (health <= 0 && !dead)
        {
			Ship ship = GetComponent<Ship>();
			if (ship && ship.IsPlayerControlled() && GameManager.instance.getMulti () > 1)
			{
				GameManager.instance.DecreaseMulti ();
			}
			else
			{
				onDie.Invoke();
				dead = true; // Prevent multiple die before destroy
				ship.Destroy ();
			}
        }
        else
        {
            if (health == 1 )
            {
                Ship ship = GetComponent<Ship>();
                if (ship && ship.IsPlayerControlled())
				{
					FMODUnity.RuntimeManager.PlayOneShot ("event:/lowhealth", ship.transform.position);
					//More screenshake!
					GameManager.instance.MainCameraController.Shake (0.2f); //0.2f + value from Blink
				}
            }

        }



    }
}
