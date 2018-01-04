using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Range(0, 100)] public int health = 100;
    bool dead = false;

    public bool immortal = false;

    public UnityEvent onDie;

    private void Start()
    {
        if (immortal)
            health = 0;
    }

    public void takeDamage( int damage )
    {
        //Imoortal objects don't take damage
        if( ! immortal )
        {
            health -= damage;
            if (health <= 0 && !dead)
            {
                onDie.Invoke();
                dead = true; // Prevent multiple die before destroy
                //Destroy the object
                Destroy(this.gameObject);
            }
        }

            
    }
}
